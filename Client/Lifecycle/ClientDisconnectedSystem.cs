using Plugins.ECSPowerNetcode.Client.Groups;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Client.Lifecycle
{
    [UpdateInGroup(typeof(ClientConnectionSystemGroup))]
    public class ClientDisconnectedSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkStreamDisconnected>()
                .ForEach((Entity entity, ref NetworkStreamDisconnected disconnected, ref CommandTargetComponent commandTargetComponent) =>
                {
                    PostUpdateCommands.DestroyEntity(commandTargetComponent.targetEntity);
                    ClientManager.Instance.OnDisconnectedFromServer();

                    Debug.Log($"[Client] Disconnected from server. Reason: {disconnected.Reason}");
                });
        }
    }
}