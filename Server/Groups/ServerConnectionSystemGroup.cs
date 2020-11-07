using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(Unity.NetCode.ServerSimulationSystemGroup))]
    public class ServerConnectionSystemGroup : ComponentSystemGroup
    {
        
    }
}