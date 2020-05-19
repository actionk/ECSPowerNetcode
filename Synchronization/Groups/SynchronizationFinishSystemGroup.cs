using Plugins.ECSPowerNetcode.Server.Groups;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Synchronization.Groups
{
    [UpdateInGroup(typeof(ServerNetworkEntitySynchronizationSystemGroup))]
    [UpdateAfter(typeof(SynchronizationProcessSystemGroup))]
    public class SynchronizationFinishSystemGroup : ComponentSystemGroup
    {
    }
}