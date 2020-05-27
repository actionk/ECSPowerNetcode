using System;
using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Shared;

namespace Plugins.ECSPowerNetcode.Client.Entities
{
    public abstract class ClientNetworkEntityBuilder<T> : EntityBuilder<T> where T: EntityBuilder<T>
    {
        protected readonly ulong networkEntityId;

        protected ClientNetworkEntityBuilder(ulong networkEntityId)
        {
            this.networkEntityId = networkEntityId;

            preBuild += OnPreBuild;
        }

        private void OnPreBuild()
        {
            if (networkEntityId == 0)
                throw new NotImplementedException("networkEntityId isn't set");

            AddComponentData(new NetworkEntity {networkEntityId = networkEntityId});
        }
    }
}