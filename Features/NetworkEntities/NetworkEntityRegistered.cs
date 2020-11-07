using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Features.NetworkEntities
{
    public struct NetworkEntityRegistered : ISystemStateComponentData
    {
        public uint networkEntityId;
    }
}