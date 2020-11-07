using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerNetworkEntitySynchronizationSystemGroup))]
    public class ServerGameSimulationSystemGroup : ComponentSystemGroup
    {
    }
}