using System.Collections.Generic;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Shared
{
    public class DefaultNetworkEntityManager : INetworkEntityManager
    {
        protected readonly Dictionary<ulong, Entity> m_entities = new Dictionary<ulong, Entity>();

        public void Add(ulong networkEntityId, Entity entity)
        {
            m_entities[networkEntityId] = entity;
        }

        public Entity GetEntityByNetworkEntityId(ulong networkEntityId)
        {
            if (!m_entities.TryGetValue(networkEntityId, out var entity))
                return Entity.Null;

            return entity;
        }

        public bool Remove(ulong networkEntityId)
        {
            return m_entities.Remove(networkEntityId);
        }

        public Entity this[ulong networkEntityId] => GetEntityByNetworkEntityId(networkEntityId);

        public Dictionary<ulong, Entity> All => m_entities;
    }
}