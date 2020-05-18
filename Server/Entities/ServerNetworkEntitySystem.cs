using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    [UpdateInGroup(typeof(ServerNetworkEntitySystemGroup))]
    public class ServerNetworkEntitySystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkEntity>()
                .WithNone<NetworkEntityRegistered>()
                .ForEach((Entity entity, ref NetworkEntity networkEntity) =>
                {
                    ServerManager.Instance.Register(networkEntity.networkEntityId, entity);

                    PostUpdateCommands.AddComponent<NetworkEntityRegistered>(entity);
                    PostUpdateCommands.AddComponent<TransferNetworkEntityToAllClients>(entity);
                });
        }
    }
}