using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerNetworkEntitySystemGroup))]
    public class ServerNetworkEntitySynchronizationSystemGroup : ComponentSystemGroup
    {
    }
}