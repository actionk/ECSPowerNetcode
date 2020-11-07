using Plugins.Shared.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.Shared.ECSPowerNetcode.Server.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Features.Synchronization.Generic
{
    [UpdateInGroup(typeof(ServerNetworkEntitySynchronizationSystemGroup))]
    public abstract class ServerSyncComponentSystem<TComponent, TConverter> : JobComponentSystem
        where TConverter : struct, ISyncEntityConverter<TComponent>
        where TComponent : struct, IComponentData
    {
        private RpcQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>, CopyEntityComponentRpcCommand<TComponent, TConverter>> m_rpcQueue;
        private EntityQuery m_updatedComponentsQuery;
        private EntityQuery m_connectionsQuery;

        protected override void OnCreate()
        {
            m_rpcQueue = World.GetExistingSystem<RpcSystem>()
                .GetRpcQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>, CopyEntityComponentRpcCommand<TComponent, TConverter>>();

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
        }

        [BurstCompile]
        struct UpdateJob : IJobChunk
        {
            [ReadOnly]
            public ComponentTypeHandle<NetworkEntity> NetworkEntity;

            [ReadOnly]
            public ComponentTypeHandle<TComponent> Component;

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
            public BufferTypeHandle<OutgoingRpcDataStreamBufferComponent> OutgoingRpcDataStreamBufferComponent;

            public RpcQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>, CopyEntityComponentRpcCommand<TComponent, TConverter>> RpcQueue;

            [ReadOnly]
            public ComponentDataFromEntity<GhostComponent> GhostComponent;

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
                        RpcQueue.Schedule(chunkConnections[i], GhostComponent, command);
                    }
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var commandsToSend = new NativeQueue<CopyEntityComponentRpcCommand<TComponent, TConverter>>(Allocator.TempJob);
            var updateJob = new UpdateJob
            {
                NetworkEntity = GetComponentTypeHandle<NetworkEntity>(true),
                Component = GetComponentTypeHandle<TComponent>(true),
                Commands = commandsToSend.AsParallelWriter()
            };
            var updateJobDependency = JobChunkExtensions.Schedule(updateJob, m_updatedComponentsQuery, inputDeps);

            var sendJob = new SendJob
            {
                RpcQueue = m_rpcQueue,
                Commands = commandsToSend,
                GhostComponent = GetComponentDataFromEntity<GhostComponent>(true),
                OutgoingRpcDataStreamBufferComponent = GetBufferTypeHandle<OutgoingRpcDataStreamBufferComponent>()
            };
            var sendJobDependency = JobChunkExtensions.Schedule(sendJob, m_connectionsQuery, updateJobDependency);
            commandsToSend.Dispose(sendJobDependency);
            return sendJobDependency;
        }
    }
}