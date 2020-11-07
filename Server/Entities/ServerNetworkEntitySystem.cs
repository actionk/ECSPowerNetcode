using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Server.Groups;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    [UpdateInGroup(typeof(ServerNetworkEntitySystemGroup))]
    [UpdateInWorld(UpdateInWorld.TargetWorld.Server)]
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