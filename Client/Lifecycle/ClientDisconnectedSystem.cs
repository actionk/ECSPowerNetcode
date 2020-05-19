using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.UnityExtras.Logs;
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
                .ForEach((Entity entity, ref CommandTargetComponent commandTargetComponent) =>
                {
                    PostUpdateCommands.DestroyEntity(commandTargetComponent.targetEntity);
                    ClientManager.Instance.OnDisconnected();

                    Debug.Log($"[Client] Disconnected from server");
                });
        }
    }
}