using Plugins.ECSPowerNetcode.Synchronization.Groups;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Synchronization
{
    [UpdateInGroup(typeof(SynchronizationFinishSystemGroup))]
    public class SynchronizeCleanSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<Synchronize>()
                .ForEach(entity => { PostUpdateCommands.RemoveComponent<Synchronize>(entity); });
        }
    }
}