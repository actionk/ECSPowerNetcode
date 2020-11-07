using System;
using Plugins.ECSPowerNetcode.EntityBulderExtensions;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;

namespace Plugins.ECSPowerNetcode.Client.Entities
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