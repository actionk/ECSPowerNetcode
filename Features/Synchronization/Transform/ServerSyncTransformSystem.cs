using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Server.Groups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.NetCode;
using Unity.Transforms;

namespace Plugins.ECSPowerNetcode.Features.Synchronization.Transform
{
    [UpdateInGroup(typeof(ServerNetworkEntitySynchronizationSystemGroup))]
    public class ServerSyncTransformSystem : JobComponentSystem
    {
        private RpcQueue<SyncTransformFromServerToClientCommand, SyncTransformFromServerToClientCommand> m_rpcQueue;
        private EntityQuery m_updatedComponentsQuery;
        private EntityQuery m_connectionsQuery;

        protected override void OnCreate()
        {
            m_rpcQueue = World.GetExistingSystem<RpcSystem>().GetRpcQueue<SyncTransformFromServerToClientCommand, SyncTransformFromServerToClientCommand>();

            m_updatedComponentsQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<NetworkEntity>(),
                    ComponentType.ReadOnly<SyncTransformFromServerToClient>(),
                    ComponentType.ReadOnly<Synchronize>(),
                    ComponentType.ReadOnly<Translation>(),
                    ComponentType.ReadOnly<Rotation>(),
                    ComponentType.ReadOnly<Scale>()
                }
            });
            m_connectionsQuery = GetEntityQuery(ComponentType.ReadOnly<OutgoingRpcDataStreamBufferComponent>());
        }


        [BurstCompile]
        struct UpdateJob : IJobChunk
        {
            [ReadOnly]
            public ComponentTypeHandle<NetworkEntity> NetworkEntity;

            [ReadOnly]
            public ComponentTypeHandle<Translation> TranslationType;

            [ReadOnly]
            public ComponentTypeHandle<Rotation> RotationType;

            [ReadOnly]
            public ComponentTypeHandle<Scale> ScaleType;

            public NativeQueue<SyncTransformFromServerToClientCommand>.ParallelWriter Commands;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var chunkNetworkEntities = chunk.GetNativeArray(NetworkEntity);
                var chunkTranslations = chunk.GetNativeArray(TranslationType);
                var chunkRotation = chunk.GetNativeArray(RotationType);
                var chunkScale = chunk.GetNativeArray(ScaleType);
                var count = chunk.Count;

                for (var i = 0; i < count; i++)
                {
                    var command = new SyncTransformFromServerToClientCommand
                    {
                        networkEntityId = chunkNetworkEntities[i].networkEntityId,
                        position = chunkTranslations[i].Value,
                        rotation = chunkRotation[i].Value,
                        scale = chunkScale[i].Value
                    };
                    Commands.Enqueue(command);
                }
            }
        }

        // [BurstCompile] // RpcQueue.Schedule uses try/catch
        struct SendJob : IJobChunk
        {
            public BufferTypeHandle<OutgoingRpcDataStreamBufferComponent> OutgoingRpcDataStreamBufferComponent;

            [ReadOnly]
            public ComponentDataFromEntity<GhostComponent> GhostComponent;

            public RpcQueue<SyncTransformFromServerToClientCommand, SyncTransformFromServerToClientCommand> RpcQueue;

            [ReadOnly]
            public NativeQueue<SyncTransformFromServerToClientCommand> Commands;

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
            var commandsToSend = new NativeQueue<SyncTransformFromServerToClientCommand>(Allocator.TempJob);
            var updateJob = new UpdateJob
            {
                NetworkEntity = GetComponentTypeHandle<NetworkEntity>(true),
                TranslationType = GetComponentTypeHandle<Translation>(true),
                RotationType = GetComponentTypeHandle<Rotation>(true),
                ScaleType = GetComponentTypeHandle<Scale>(true),
                Commands = commandsToSend.AsParallelWriter()
            };
            var updateJobDependency = updateJob.Schedule(m_updatedComponentsQuery, inputDeps);

            var sendJob = new SendJob
            {
                RpcQueue = m_rpcQueue,
                Commands = commandsToSend,
                GhostComponent = GetComponentDataFromEntity<GhostComponent>(true),
                OutgoingRpcDataStreamBufferComponent = GetBufferTypeHandle<OutgoingRpcDataStreamBufferComponent>()
            };
            var sendJobDependency = sendJob.Schedule(m_connectionsQuery, updateJobDependency);
            commandsToSend.Dispose(sendJobDependency);
            return sendJobDependency;
        }
    }
}