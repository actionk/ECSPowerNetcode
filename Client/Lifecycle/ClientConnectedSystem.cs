using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Client.Components;
using Plugins.ECSPowerNetcode.Client.Groups;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Client.Lifecycle
{
    [UpdateInGroup(typeof(ClientConnectionSystemGroup))]
    [UpdateAfter(typeof(ClientConnectToServerSystem))]
    public class ClientConnectedSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithNone<NetworkStreamInGame>()
                .ForEach((Entity connectionEntity, ref NetworkIdComponent id) =>
                {
                    Debug.Log($"[Client] Connected to server. Network id = [{id.Value}]");

                    EntityWrapper.Wrap(connectionEntity, EntityManager)
                        .SetName($"ServerConnection_{id.Value}");

                    var commandHandler = EntityWrapper.CreateEntity(EntityManager)
                        .AddComponent<ClientToServerCommandHandler>()
                        .SetName($"ServerConnection_{id.Value}_CommandHandler")
                        .Entity;

                    ClientManager.Instance.OnConnectedToServer(connectionEntity, commandHandler, id.Value);

                    PostUpdateCommands.SetComponent(connectionEntity, new CommandTargetComponent {targetEntity = commandHandler});
                    PostUpdateCommands.AddComponent<NetworkStreamInGame>(connectionEntity);
                });
        }
    }
}