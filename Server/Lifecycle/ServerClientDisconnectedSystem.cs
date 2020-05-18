using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.UnityExtras.Logs;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Lifecycle
{
    [UpdateInGroup(typeof(ServerConnectionSystemGroup))]
    public class ServerClientDisconnectedSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkStreamDisconnected>()
                .ForEach((Entity entity, ref NetworkIdComponent networkIdComponent, ref CommandTargetComponent commandTargetComponent) =>
                {
                    PostUpdateCommands.DestroyEntity(commandTargetComponent.targetEntity);
                    ServerManager.Instance.OnDisconnected(entity);

                    UnityLogger.Info($"[Server] Client disconnected from server with network id = [{networkIdComponent.Value}]");
                });
        }
    }
}