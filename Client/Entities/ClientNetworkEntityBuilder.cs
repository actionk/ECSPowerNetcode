using System;
using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;

namespace Plugins.ECSPowerNetcode.Client.Entities
{
    public abstract class ClientNetworkEntityBuilder<T> : EntityBuilder<T> where T : EntityBuilder<T>
    {
        protected ClientNetworkEntityBuilder(uint networkEntityId)
        {
            if (networkEntityId == 0)
                throw new NotImplementedException("networkEntityId isn't set");

            AddComponentData(new NetworkEntity {networkEntityId = networkEntityId});
        }
    }
}