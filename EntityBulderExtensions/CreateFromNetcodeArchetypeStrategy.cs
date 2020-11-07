using Plugins.ECSEntityBuilder;
using Plugins.ECSEntityBuilder.Archetypes;
using Plugins.ECSEntityBuilder.InstantiationStrategy;
using Plugins.ECSEntityBuilder.Variables;
using Plugins.ECSPowerNetcode.Archetypes;
using Plugins.ECSPowerNetcode.Worlds;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.EntityBulderExtensions
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