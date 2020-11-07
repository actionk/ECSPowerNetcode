using Plugins.Shared.ECSPowerNetcode.Client.Groups;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.Shared.ECSPowerNetcode.Client.Lifecycle
{
    [UpdateInGroup(typeof(ClientConnectionSystemGroup))]
    public class ClientDisconnectedSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkStreamDisconnected>()
                .ForEach((Entity entity, ref CommandTargetComponent commandTargetComponent) =>
                {
                    PostUpdateCommands.DestroyEntity(commandTargetComponent.targetEntity);
                    ClientManager.Instance.OnDisconnectedFromServer();

                    Debug.Log($"[Client] Disconnected from server");
                });
        }
    }
}