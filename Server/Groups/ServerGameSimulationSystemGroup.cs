using Unity.Entities;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerNetworkEntitySynchronizationSystemGroup))]
    public class ServerGameSimulationSystemGroup : ComponentSystemGroup
    {
    }
}