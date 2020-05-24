using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Client.Entities
{
    [UpdateInGroup(typeof(ClientNetworkEntitySystemGroup))]
    public class ClientNetworkEntitySystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkEntity>()
                .WithNone<NetworkEntityRegistered>()
                .ForEach((Entity entity, ref NetworkEntity networkEntity) =>
                {
                    ClientManager.Instance.Register(networkEntity.networkEntityId, entity);
                    PostUpdateCommands.AddComponent(entity, new NetworkEntityRegistered {networkEntityId = networkEntity.networkEntityId});
                });

            Entities
                .WithAll<NetworkEntityRegistered>()
                .WithNone<NetworkEntity>()
                .ForEach((Entity entity, ref NetworkEntityRegistered networkEntity) =>
                {
                    ClientManager.Instance.Remove(networkEntity.networkEntityId);
                    PostUpdateCommands.RemoveComponent<NetworkEntityRegistered>(entity);
                });
        }
    }
}