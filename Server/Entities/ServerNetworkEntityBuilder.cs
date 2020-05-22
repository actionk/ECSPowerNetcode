using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Shared;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    public abstract class ServerNetworkEntityBuilder<T> : EntityBuilder<T> where T : EntityBuilder<T>
    {
        protected override void OnPreBuild(EntityManagerWrapper wrapper)
        {
            var networkEntityId = ServerManager.Instance.NextNetworkEntityId;
            AddComponentData(new NetworkEntity {networkEntityId = networkEntityId});
        }
    }
}