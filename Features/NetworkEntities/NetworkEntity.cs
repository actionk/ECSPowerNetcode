using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Features.NetworkEntities
{
    public struct NetworkEntity : IComponentData
    {
        public uint networkEntityId;
    }
}