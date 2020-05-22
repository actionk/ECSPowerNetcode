#if ENABLE_ECS_POWER_NETCODE_DEBUGGER

using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Debugging
{
    public class ValidateHangingRpcCommandsSystem : ComponentSystem
    {
        private struct RpcCommandAdded : ISystemStateComponentData
        {
            public double timeAdded;
        }

        private struct RpcCommandUserNotified : ISystemStateComponentData
        {
        }

        protected override void OnUpdate()
        {
            Entities
                .WithAll<ReceiveRpcCommandRequestComponent, RpcCommandAdded>()
                .WithNone<RpcCommandUserNotified>()
                .ForEach((Entity entity, ref RpcCommandAdded rpcCommandAdded) =>
                {
                    if (Time.ElapsedTime - rpcCommandAdded.timeAdded > 1.0f)
                    {
                        Debug.LogWarning($"[NetworkDebugger] There is a hanging RPC entity: {entity} in {World.Name} world");
                        PostUpdateCommands.AddComponent<RpcCommandUserNotified>(entity);
                    }
                });

            Entities
                .WithAll<ReceiveRpcCommandRequestComponent>()
                .WithNone<RpcCommandAdded>()
                .ForEach(entity =>
                {
                    PostUpdateCommands.AddComponent(entity, new RpcCommandAdded
                    {
                        timeAdded = Time.ElapsedTime
                    });
                });

            Entities
                .WithAll<RpcCommandAdded>()
                .WithNone<ReceiveRpcCommandRequestComponent>()
                .ForEach(entity => { PostUpdateCommands.RemoveComponent<RpcCommandAdded>(entity); });
        }
    }
}

#endif