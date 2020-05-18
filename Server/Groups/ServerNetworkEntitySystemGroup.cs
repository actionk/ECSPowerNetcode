using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerRequestProcessingSystemGroup))]
    public class ServerNetworkEntitySystemGroup : ComponentSystemGroup
    {
        
    }
}