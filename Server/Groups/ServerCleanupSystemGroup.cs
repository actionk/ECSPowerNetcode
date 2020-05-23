using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerGameSimulationSystemGroup))]
    public class ServerCleanupSystemGroup : ComponentSystemGroup
    {
    }
}