using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Shared;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    public abstract class ServerNetworkEntityBuilder<T> : EntityBuilder<T> where T : EntityBuilder<T>
    {
        protected ServerNetworkEntityBuilder()
        {
            var networkEntityId = ServerManager.Instance.NextNetworkEntityId;
            AddComponentData(new NetworkEntity {networkEntityId = networkEntityId});
        }

        protected ServerNetworkEntityBuilder(ulong networkEntityId)
        {
            AddComponentData(new NetworkEntity {networkEntityId = networkEntityId});
        }
    }
}