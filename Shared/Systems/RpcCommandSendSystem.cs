using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Shared.Systems
{
    public abstract class RpcCommandSendSystem<TActionSerializer, TActionRequest> : RpcCommandRequestSystem<TActionSerializer, TActionRequest>
        where TActionSerializer : struct, IRpcCommandSerializer<TActionRequest>
        where TActionRequest : struct, IComponentData
    {
        [BurstCompile]
        struct SendJob : IJobEntityBatch
        {
            public SendRpcData data;

            public void Execute(ArchetypeChunk chunk, int orderIndex)
            {
                data.Execute(chunk, orderIndex);
            }
        }

        protected override void OnUpdate()
        {
            var sendJob = new SendJob {data = InitJobData()};
            ScheduleJobData(sendJob);
        }
    }
}