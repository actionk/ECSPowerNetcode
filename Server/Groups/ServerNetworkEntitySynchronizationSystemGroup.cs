using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(Unity.NetCode.ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerNetworkEntitySystemGroup))]
    public class ServerNetworkEntitySynchronizationSystemGroup : ComponentSystemGroup
    {
    }
}