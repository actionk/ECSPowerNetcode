using Plugins.Shared.ECSPowerNetcode.EntityBulderExtensions;
using Plugins.Shared.ECSPowerNetcode.Features.NetworkEntities;

namespace Plugins.Shared.ECSPowerNetcode.Server.Entities
{
    public abstract class ServerNetworkEntityBuilder : NetcodeEntityBuilder
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