using System.Collections.Generic;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Shared
{
    public interface INetworkEntityManager
    {
        void Add(ulong networkEntityId, Entity entity);
        Entity GetEntityByNetworkEntityId(ulong networkEntityId);
        bool Remove(ulong networkEntityId);

        Dictionary<ulong, Entity> All { get; }
    }
}