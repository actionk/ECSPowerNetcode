using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.UnityExtras.Logs;
using Unity.Entities;
using Unity.NetCode;

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
                    UnityLogger.Info($"[Server] Client connected with network id = [{networkIdComponent.Value}]");

                    var connectionCommandHandler = EntityManager.CreateEntity();
                    PostUpdateCommands.AddComponent<ServerToClientCommandHandler>(connectionCommandHandler);

                    PostUpdateCommands.SetComponent(connectionEntity, new CommandTargetComponent {targetEntity = connectionCommandHandler});
                    PostUpdateCommands.AddComponent<NetworkStreamInGame>(connectionEntity);

                    ServerManager.Instance.OnConnected(networkIdComponent.Value, connectionEntity, connectionCommandHandler);
                });
        }
    }
}