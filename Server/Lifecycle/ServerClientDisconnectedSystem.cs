using Plugins.ECSPowerNetcode.Server.Groups;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Server.Lifecycle
{
    [UpdateInGroup(typeof(ServerConnectionSystemGroup))]
    [UpdateInWorld(UpdateInWorld.TargetWorld.Server)]
    public class ServerClientDisconnectedSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkStreamDisconnected>()
                .ForEach((Entity entity, ref NetworkIdComponent networkIdComponent, ref CommandTargetComponent commandTargetComponent) =>
                {
                    PostUpdateCommands.DestroyEntity(commandTargetComponent.targetEntity);
                    ServerManager.Instance.OnDisconnected(networkIdComponent.Value);

                    Debug.Log($"[Server] Client disconnected from server with network id = [{networkIdComponent.Value}]");
                });
        }
    }
}