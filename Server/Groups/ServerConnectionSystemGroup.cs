using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    public class ServerConnectionSystemGroup : ComponentSystemGroup
    {
        
    }
}