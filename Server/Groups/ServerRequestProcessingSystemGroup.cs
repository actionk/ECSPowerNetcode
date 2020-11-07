using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(Unity.NetCode.ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerConnectionSystemGroup))]
    public class ServerRequestProcessingSystemGroup : ComponentSystemGroup
    {
    }
}