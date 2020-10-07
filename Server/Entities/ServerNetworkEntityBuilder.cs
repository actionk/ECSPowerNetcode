using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    public abstract class ServerNetworkEntityBuilder<T> : EntityBuilder<T> where T : EntityBuilder<T>
    {
        public uint NetworkEntityId { get; }

        protected ServerNetworkEntityBuilder()
        {
            NetworkEntityId = ServerManager.Instance.NextNetworkEntityId;
            AddComponentData(new NetworkEntity {networkEntityId = NetworkEntityId});
        }

        protected ServerNetworkEntityBuilder(uint networkEntityId)
        {
            AddComponentData(new NetworkEntity {networkEntityId = networkEntityId});
        }
    }
}