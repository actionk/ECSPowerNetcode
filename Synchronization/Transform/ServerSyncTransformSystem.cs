using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.NetCode;
using Unity.Transforms;

namespace Plugins.ECSPowerNetcode.Synchronization.Transform
{
    [UpdateInGroup(typeof(ServerNetworkEntitySystemGroup))]
    public class ServerSyncTransformSystem : JobComponentSystem
    {
        private RpcQueue<SyncTransformFromServerToClientCommand> m_rpcQueue;
        private EntityQuery m_updatedComponentsQuery;
        private EntityQuery m_connectionsQuery;
        private EndSimulationEntityCommandBufferSystem m_entityCommandBufferSource;

        protected override void OnCreate()
        {
            m_rpcQueue = World.GetExistingSystem<RpcSystem>().GetRpcQueue<SyncTransformFromServerToClientCommand>();

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
            m_entityCommandBufferSource = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
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
            public ArchetypeChunkComponentType<Translation> TranslationType;

            [ReadOnly]
            public ArchetypeChunkComponentType<Rotation> RotationType;

            [ReadOnly]
            public ArchetypeChunkComponentType<Scale> ScaleType;

            public NativeQueue<SyncTransformFromServerToClientCommand>.ParallelWriter Commands;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var chunkEntities = chunk.GetNativeArray(Entities);
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

                    CommandBuffer.RemoveComponent<Synchronize>(m_nativeThreadIndex, chunkEntities[i]);
                }
            }
        }

        // [BurstCompile] // RpcQueue.Schedule uses try/catch
        struct SendJob : IJobChunk
        {
            public ArchetypeChunkBufferType<OutgoingRpcDataStreamBufferComponent> OutgoingRpcDataStreamBufferComponent;

            public RpcQueue<SyncTransformFromServerToClientCommand> RpcQueue;

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
                        RpcQueue.Schedule(chunkConnections[i], command);
                    }
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var commandsToSend = new NativeQueue<SyncTransformFromServerToClientCommand>(Allocator.TempJob);
            var updateJob = new UpdateJob
            {
                CommandBuffer = m_entityCommandBufferSource.CreateCommandBuffer().ToConcurrent(),
                Entities = GetArchetypeChunkEntityType(),
                NetworkEntity = GetArchetypeChunkComponentType<NetworkEntity>(true),
                TranslationType = GetArchetypeChunkComponentType<Translation>(true),
                RotationType = GetArchetypeChunkComponentType<Rotation>(true),
                ScaleType = GetArchetypeChunkComponentType<Scale>(true),
                Commands = commandsToSend.AsParallelWriter()
            };

            var updateJobDependency = updateJob.Schedule(m_updatedComponentsQuery, inputDeps);
            m_entityCommandBufferSource.AddJobHandleForProducer(updateJobDependency);

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