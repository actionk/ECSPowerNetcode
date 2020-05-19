using Plugins.ECSPowerNetcode.Client.Components;
using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.UnityExtras.Logs;
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
                    Debug.Log($"[Client] Connection to server established with network id = [{id.Value}]");

                    var commandHandler = EntityManager.CreateEntity();
                    PostUpdateCommands.AddComponent(commandHandler, new ClientToServerCommandHandler());

                    ClientManager.Instance.OnConnectionEstablished(connectionEntity, commandHandler, id.Value);

                    PostUpdateCommands.SetComponent(connectionEntity, new CommandTargetComponent {targetEntity = commandHandler});
                    PostUpdateCommands.AddComponent<NetworkStreamInGame>(connectionEntity);
                });
        }
    }
}