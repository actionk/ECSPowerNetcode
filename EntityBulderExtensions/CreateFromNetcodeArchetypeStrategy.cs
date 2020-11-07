using Plugins.Shared.ECSEntityBuilder;
using Plugins.Shared.ECSEntityBuilder.Archetypes;
using Plugins.Shared.ECSEntityBuilder.InstantiationStrategy;
using Plugins.Shared.ECSEntityBuilder.Variables;
using Plugins.Shared.ECSPowerNetcode.Archetypes;
using Plugins.Shared.ECSPowerNetcode.Worlds;
using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.EntityBulderExtensions
{
    public class CreateFromNetcodeArchetypeStrategy<T> : IEntityCreationStrategy where T : IArchetypeDescriptor
    {
        private readonly WorldType m_worldType;

        public CreateFromNetcodeArchetypeStrategy(WorldType worldType)
        {
            m_worldType = worldType;
        }

        public Entity Create(EntityManagerWrapper wrapper, EntityVariableMap variables)
        {
            return wrapper.CreateEntity(NetcodeEntityArchetypeManager.Instance.GetOrCreateArchetype<T>(m_worldType));
        }
    }
}