using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Synchronization.Generic
{
    [UpdateInGroup(typeof(ServerNetworkEntitySystemGroup))]
    public abstract class ServerSyncComponentSystem<TComponent, TConverter> : JobComponentSystem
        where TConverter : struct, ISyncEntityConverter<TComponent>
        where TComponent : struct, IComponentData
    {
        private RpcQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>> m_rpcQueue;
        private EntityQuery m_updatedComponentsQuery;
        private EntityQuery m_connectionsQuery;
        private EndSimulationEntityCommandBufferSystem m_EntityCommandBufferSource;

        protected override void OnCreate()
        {
            m_rpcQueue = World.GetExistingSystem<RpcSystem>().GetRpcQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>>();

            m_updatedComponentsQuery = GetEntityQuery(
                new EntityQueryDesc
                {
                    All = new[]
                    {
                        ComponentType.ReadOnly<NetworkEntity>(),
                        ComponentType.ReadOnly<Synchronize>(),
                        ComponentType.ReadOnly<TComponent>()
                    }
                }
            );

            m_connectionsQuery = GetEntityQuery(ComponentType.ReadOnly<OutgoingRpcDataStreamBufferComponent>());
            m_EntityCommandBufferSource = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        [BurstCompile]
        struct UpdateJob : IJobChunk
        {
            [NativeSetThreadIndex]
            internal int m_nativeThreadIndex;

            public EntityCommandBuffer.Concurrent CommandBuffer;

            [ReadOnly]
            public ArchetypeChunkEntityType Entities;

            [ReadOnly]
            public ArchetypeChunkComponentType<NetworkEntity> NetworkEntity;

            [ReadOnly]
            public ArchetypeChunkComponentType<TComponent> Component;

            public NativeQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>>.ParallelWriter Commands;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var chunkEntities = chunk.GetNativeArray(Entities);
                var chunkNetworkEntities = chunk.GetNativeArray(NetworkEntity);
                var chunkComponents = chunk.GetNativeArray(Component);
                var count = chunk.Count;

                for (var i = 0; i < count; i++)
                {
                    TConverter converter = new TConverter();
                    converter.Convert(chunkComponents[i]);

                    var copyEntityComponentRpcCommand = new CopyEntityComponentRpcCommand<TComponent, TConverter>
                    {
                        networkEntityId = chunkNetworkEntities[i].networkEntityId,
                        component = converter
                    };
                    Commands.Enqueue(copyEntityComponentRpcCommand);

                    CommandBuffer.RemoveComponent<Synchronize>(m_nativeThreadIndex, chunkEntities[i]);
                }
            }
        }

        // [BurstCompile] // RpcQueue.Schedule uses try/catch
        struct SendJob : IJobChunk
        {
            public ArchetypeChunkBufferType<OutgoingRpcDataStreamBufferComponent> OutgoingRpcDataStreamBufferComponent;

            public RpcQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>> RpcQueue;

            [ReadOnly]
            public NativeQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>> Commands;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var chunkConnections = chunk.GetBufferAccessor(OutgoingRpcDataStreamBufferComponent);
                var count = chunk.Count;

                var commands = Commands.ToArray(Allocator.Temp);
                foreach (var command in commands)
                {
                    for (var i = 0; i < count; i++)
                    {
                        RpcQueue.Schedule(chunkConnections[i], command);
                    }
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var commandsToSend = new NativeQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>>(Allocator.TempJob);
            var updateJob = new UpdateJob
            {
                CommandBuffer = m_EntityCommandBufferSource.CreateCommandBuffer().ToConcurrent(),
                Entities = GetArchetypeChunkEntityType(),
                NetworkEntity = GetArchetypeChunkComponentType<NetworkEntity>(true),
                Component = GetArchetypeChunkComponentType<TComponent>(true),
                Commands = commandsToSend.AsParallelWriter()
            };
            var updateJobDependency = updateJob.Schedule(m_updatedComponentsQuery, inputDeps);

            m_EntityCommandBufferSource.AddJobHandleForProducer(updateJobDependency);

            var sendJob = new SendJob
            {
                RpcQueue = m_rpcQueue,
                Commands = commandsToSend,
                OutgoingRpcDataStreamBufferComponent = GetArchetypeChunkBufferType<OutgoingRpcDataStreamBufferComponent>()
            };
            var sendJobDependency = sendJob.Schedule(m_connectionsQuery, updateJobDependency);
            commandsToSend.Dispose(sendJobDependency);
            return sendJobDependency;
        }
    }
}