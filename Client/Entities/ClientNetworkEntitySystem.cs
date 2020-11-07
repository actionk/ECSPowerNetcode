using Plugins.Shared.ECSPowerNetcode.Client.Groups;
using Plugins.Shared.ECSPowerNetcode.Features.NetworkEntities;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Client.Entities
{
    [UpdateInGroup(typeof(ClientNetworkEntitySystemGroup))]
    [UpdateInWorld(UpdateInWorld.TargetWorld.Client)]
    public class ClientNetworkEntitySystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkEntity>()
                .WithNone<NetworkEntityRegistered>()
                .ForEach((Entity entity, ref NetworkEntity networkEntity) =>
                {
                    ClientManager.Instance.NetworkEntityManager.Add(networkEntity.networkEntityId, entity);
                    PostUpdateCommands.AddComponent(entity, new NetworkEntityRegistered {networkEntityId = networkEntity.networkEntityId});
                });

            Entities
                .WithAll<NetworkEntityRegistered>()
                .WithNone<NetworkEntity>()
                .ForEach((Entity entity, ref NetworkEntityRegistered networkEntity) =>
                {
                    ClientManager.Instance.NetworkEntityManager.Remove(networkEntity.networkEntityId);
                    PostUpdateCommands.RemoveComponent<NetworkEntityRegistered>(entity);
                });
        }
    }
}