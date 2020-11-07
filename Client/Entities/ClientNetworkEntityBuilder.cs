using System;
using Plugins.Shared.ECSPowerNetcode.EntityBulderExtensions;
using Plugins.Shared.ECSPowerNetcode.Features.NetworkEntities;

namespace Plugins.Shared.ECSPowerNetcode.Client.Entities
{
    public abstract class ClientNetworkEntityBuilder : NetcodeEntityBuilder
    {
        protected ClientNetworkEntityBuilder(uint networkEntityId)
        {
            if (networkEntityId == 0)
                throw new NotImplementedException("networkEntityId isn't set");

            AddComponentData(new NetworkEntity {networkEntityId = networkEntityId});
        }
    }
}