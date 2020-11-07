using System.Collections.Generic;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Shared
{
    public class DefaultNetworkEntityManager : INetworkEntityManager
    {
        protected readonly Dictionary<uint, Entity> entities = new Dictionary<uint, Entity>();

        public void Add(uint networkEntityId, Entity entity)
        {
            entities[networkEntityId] = entity;
        }

        public Entity TryGetEntityByNetworkEntityId(uint networkEntityId)
        {
            if (!entities.TryGetValue(networkEntityId, out var entity))
                return Entity.Null;

            return entity;
        }

        public Entity GetEntityByNetworkEntityId(uint networkEntityId)
        {
            if (!entities.TryGetValue(networkEntityId, out var entity))
                throw new KeyNotFoundException($"No network entity with {networkEntityId} found");

            return entity;
        }

        public bool Remove(uint networkEntityId)
        {
            return entities.Remove(networkEntityId);
        }

        public Entity this[uint networkEntityId] => TryGetEntityByNetworkEntityId(networkEntityId);

        public Dictionary<uint, Entity> All => entities;
    }
}