using System.Collections.Generic;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Shared
{
    public class DefaultNetworkEntityManager : INetworkEntityManager
    {
        protected readonly Dictionary<uint, Entity> m_entities = new Dictionary<uint, Entity>();

        public void Add(uint networkEntityId, Entity entity)
        {
            m_entities[networkEntityId] = entity;
        }

        public Entity GetEntityByNetworkEntityId(uint networkEntityId)
        {
            if (!m_entities.TryGetValue(networkEntityId, out var entity))
                return Entity.Null;

            return entity;
        }

        public bool Remove(uint networkEntityId)
        {
            return m_entities.Remove(networkEntityId);
        }

        public Entity this[uint networkEntityId] => GetEntityByNetworkEntityId(networkEntityId);

        public Dictionary<uint, Entity> All => m_entities;
    }
}