using Plugins.Shared.Netcode.Entities.Groups.Server;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Groups
{
    [UpdateInGroup(typeof(ServerSimulationSystemGroup))]
    [UpdateAfter(typeof(ServerRequestProcessingSystemGroup))]
    public class ServerNetworkEntitySystemGroup : ComponentSystemGroup
    {
    }
}