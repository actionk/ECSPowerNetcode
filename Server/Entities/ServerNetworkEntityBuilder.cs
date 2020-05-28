using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Shared;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    public abstract class ServerNetworkEntityBuilder<T> : EntityBuilder<T> where T : EntityBuilder<T>
    {
        public ulong NetworkEntityId { get; }

        protected ServerNetworkEntityBuilder()
        {
            NetworkEntityId = ServerManager.Instance.NextNetworkEntityId;
            AddComponentData(new NetworkEntity {networkEntityId = NetworkEntityId});
        }

        protected ServerNetworkEntityBuilder(ulong networkEntityId)
        {
            AddComponentData(new NetworkEntity {networkEntityId = networkEntityId});
        }
    }
}