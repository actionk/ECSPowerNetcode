using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(Unity.NetCode.ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerGameSimulationSystemGroup))]
    public class ServerCleanupSystemGroup : ComponentSystemGroup
    {
    }
}