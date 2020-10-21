using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(Unity.NetCode.ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerRequestProcessingSystemGroup))]
    public class ServerNetworkEntitySystemGroup : ComponentSystemGroup
    {
        
    }
}