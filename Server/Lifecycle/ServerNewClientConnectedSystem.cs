using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Server.Lifecycle
{
    [UpdateInGroup(typeof(ServerConnectionSystemGroup))]
    [UpdateAfter(typeof(ServerStartSystem))]
    public class ServerNewClientConnectedSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkIdComponent>()
                .WithNone<NetworkStreamInGame>()
                .ForEach((Entity connectionEntity, ref NetworkIdComponent networkIdComponent) =>
                {
                    Debug.Log($"[Server] Client connected with network id = [{networkIdComponent.Value}]");

                    EntityWrapper.Wrap(connectionEntity, EntityManager)
                        .SetName($"ClientConnection_{networkIdComponent.Value}");

                    var connectionCommandHandler = EntityWrapper.CreateEntity(EntityManager)
                        .AddComponent<ServerToClientCommandHandler>()
                        .SetName($"ClientConnection_{networkIdComponent.Value}_CommandBuffer")
                        .Entity;

                    PostUpdateCommands.SetComponent(connectionEntity, new CommandTargetComponent {targetEntity = connectionCommandHandler});
                    PostUpdateCommands.AddComponent<NetworkStreamInGame>(connectionEntity);

                    ServerManager.Instance.OnConnected(networkIdComponent.Value, connectionEntity, connectionCommandHandler);
                });
        }
    }
}