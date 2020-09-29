using System.Collections.Generic;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Shared
{
    public interface INetworkEntityManager
    {
        void Add(uint networkEntityId, Entity entity);
        Entity GetEntityByNetworkEntityId(uint networkEntityId);
        bool Remove(uint networkEntityId);

        Dictionary<uint, Entity> All { get; }
    }
}