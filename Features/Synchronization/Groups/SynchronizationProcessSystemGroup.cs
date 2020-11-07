using Plugins.Shared.ECSPowerNetcode.Server.Groups;
using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Features.Synchronization.Groups
{
    [UpdateInGroup(typeof(ServerNetworkEntitySynchronizationSystemGroup))]
    public class SynchronizationProcessSystemGroup : ComponentSystemGroup
    {
        
    }
}