using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Features.NetworkEntities
{
    public struct NetworkEntity : IComponentData
    {
        public uint networkEntityId;
    }
}