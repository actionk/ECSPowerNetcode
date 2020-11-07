using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(Unity.NetCode.ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerGameSimulationSystemGroup))]
    public class ServerCleanupSystemGroup : ComponentSystemGroup
    {
    }
}