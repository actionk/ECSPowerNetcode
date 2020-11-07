using Plugins.Shared.ECSEntityBuilder;
using Plugins.Shared.ECSPowerNetcode.Worlds;
using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.EntityBulderExtensions
{
    public static class NetcodeEntityWrapper
    {
        public static EntityWrapper Wrap(Entity entity, WorldType entityWorldType)
        {
            return new EntityWrapper(entity, NetcodeEntityManagerWrapper.FromWorld(entityWorldType));
        }
    }
}