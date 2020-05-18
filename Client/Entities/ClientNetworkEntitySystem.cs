using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.ECSPowerNetcode.Shared;
using Plugins.UnityExtras.DependencyInjections;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Client.Entities
{
    [UpdateInGroup(typeof(ClientNetworkEntitySystemGroup))]
    public class ClientNetworkEntitySystem : ComponentSystem
    {
        private readonly ClientNetworkEntityManager m_networkEntityManager = DependencyProvider.Resolve<ClientNetworkEntityManager>();

        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkEntity>()
                .WithNone<NetworkEntityRegistered>()
                .ForEach((Entity entity, ref NetworkEntity networkEntity) =>
                {
                    m_networkEntityManager.Register(networkEntity.networkEntityId, entity);
                    PostUpdateCommands.AddComponent<NetworkEntityRegistered>(entity);
                });
        }
    }
}