using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Worlds;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.EntityBulderExtensions
{
    public static class NetcodeEntityWrapper
    {
        public static EntityWrapper Wrap(Entity entity, WorldType entityWorldType)
        {
            return new EntityWrapper(entity, NetcodeEntityManagerWrapper.FromWorld(entityWorldType));
        }
    }
}