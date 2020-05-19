using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Burst;
using Unity.Collections;
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

        protected override void OnCreate()
        {
            m_rpcQueue = World.GetExistingSystem<RpcSystem>().GetRpcQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>>();

            m_updatedComponentsQuery = GetEntityQuery(ComponentType.ReadOnly<NetworkEntity>(), ComponentType.ReadOnly<TComponent>());
            m_updatedComponentsQuery.SetChangedVersionFilter(typeof(TComponent));

            m_connectionsQuery = GetEntityQuery(ComponentType.ReadOnly<OutgoingRpcDataStreamBufferComponent>());
        }

        [BurstCompile]
        struct UpdateJob : IJobChunk
        {
            [ReadOnly]
            public ArchetypeChunkComponentType<NetworkEntity> NetworkEntity;

            [ReadOnly]
            public ArchetypeChunkComponentType<TComponent> Component;

            public NativeQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>>.ParallelWriter Commands;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
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
                NetworkEntity = GetArchetypeChunkComponentType<NetworkEntity>(true),
                Component = GetArchetypeChunkComponentType<TComponent>(true),
                Commands = commandsToSend.AsParallelWriter()
            };

            var updateJobDependency = updateJob.Schedule(m_updatedComponentsQuery, inputDeps);

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