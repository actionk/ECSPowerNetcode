using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Shared
{
    public struct NetworkEntity : IComponentData
    {
        public ulong networkEntityId;
    }
}