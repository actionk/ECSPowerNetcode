using Plugins.Shared.ECSEntityBuilder;
using Plugins.Shared.ECSPowerNetcode.Worlds;
using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.EntityBulderExtensions
{
    public static class NetcodeEntityManagerWrapper
    {
        public static EntityManagerWrapper FromWorld(WorldType entityWorldType)
        {
            switch (entityWorldType)
            {
                default:
                    return new EntityManagerWrapper(World.DefaultGameObjectInjectionWorld.EntityManager);
                case WorldType.CLIENT:
                    return new EntityManagerWrapper(EntityWorldManager.Instance.Client.EntityManager);
                case WorldType.SERVER:
                    return new EntityManagerWrapper(EntityWorldManager.Instance.Server.EntityManager);
            }
        }
    }
}