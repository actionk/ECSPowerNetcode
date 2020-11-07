using Plugins.Shared.ECSEntityBuilder;
using Plugins.Shared.ECSEntityBuilder.Archetypes;
using Plugins.Shared.ECSPowerNetcode.Worlds;
using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.EntityBulderExtensions
{
    public class NetcodeEntityBuilder : EntityBuilder
    {
        public new void CreateFromArchetype<T>()
            where T : IArchetypeDescriptor
        {
            SetCreationStrategy(new CreateFromNetcodeArchetypeStrategy<T>(WorldType.DEFAULT));
        }

        public new void CreateFromArchetype<T>(WorldType worldType)
            where T : IArchetypeDescriptor
        {
            SetCreationStrategy(new CreateFromNetcodeArchetypeStrategy<T>(worldType));
        }

        public Entity Build(WorldType worldType)
        {
            return Build(new EntityManagerWrapper(EntityWorldManager.Instance.GetWorldByType(worldType).EntityManager));
        }
    }
}