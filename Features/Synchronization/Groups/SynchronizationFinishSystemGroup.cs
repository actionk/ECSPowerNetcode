using Plugins.Shared.ECSPowerNetcode.Server.Groups;
using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Features.Synchronization.Groups
{
    [UpdateInGroup(typeof(ServerNetworkEntitySynchronizationSystemGroup))]
    [UpdateAfter(typeof(SynchronizationProcessSystemGroup))]
    public class SynchronizationFinishSystemGroup : ComponentSystemGroup
    {
    }
}