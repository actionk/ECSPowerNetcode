using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
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
                    ServerManager.Instance.NetworkEntityManager.Add(networkEntity.networkEntityId, entity);

                    PostUpdateCommands.AddComponent(entity, new NetworkEntityRegistered {networkEntityId = networkEntity.networkEntityId});
                    PostUpdateCommands.AddComponent<TransferNetworkEntityToAllClients>(entity);
                });

            Entities
                .WithAll<NetworkEntityRegistered>()
                .WithNone<NetworkEntity>()
                .ForEach((Entity entity, ref NetworkEntityRegistered networkEntity) =>
                {
                    ServerManager.Instance.NetworkEntityManager.Remove(networkEntity.networkEntityId);
                    PostUpdateCommands.RemoveComponent<NetworkEntityRegistered>(entity);
                });
        }
    }
}