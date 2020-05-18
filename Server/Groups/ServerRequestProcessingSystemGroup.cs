using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerConnectionSystemGroup))]
    public class ServerRequestProcessingSystemGroup : ComponentSystemGroup
    {
    }
}