using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Features.NetworkEntities
{
    public struct NetworkEntityRegistered : ISystemStateComponentData
    {
        public uint networkEntityId;
    }
}