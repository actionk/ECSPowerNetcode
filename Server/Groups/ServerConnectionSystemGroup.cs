using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(Unity.NetCode.ServerSimulationSystemGroup))]
    public class ServerConnectionSystemGroup : ComponentSystemGroup
    {
        
    }
}