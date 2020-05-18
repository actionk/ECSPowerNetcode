using System.Collections.Generic;
using Plugins.Framework.Extensions.Collections;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Shared
{
    public abstract class ANetworkEntityManager
    {
        private readonly Dictionary<ulong, Entity> m_entities = new Dictionary<ulong, Entity>();

        public void Register(ulong networkEntityId, Entity entity)
        {
            m_entities[networkEntityId] = entity;
        }

        public Entity GetEntityByNetworkEntityId(ulong networkEntityId)
        {
            return m_entities.GetValueOrDefault(networkEntityId, Entity.Null);
        }

        public Dictionary<ulong, Entity> AllNetworkEntities => m_entities;

        public Entity this[ulong index] => m_entities[index];
    }
}