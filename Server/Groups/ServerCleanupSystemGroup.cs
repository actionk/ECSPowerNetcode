using Plugins.Shared.Netcode.Entities.Groups.Server;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerGameSimulationSystemGroup))]
    public class ServerCleanupSystemGroup : ComponentSystemGroup
    {
    }
}