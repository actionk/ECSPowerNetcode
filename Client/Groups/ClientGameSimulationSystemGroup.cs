using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    [UpdateAfter(typeof(ClientNetworkEntitySystemGroup))]
    public class ClientGameSimulationSystemGroup : ComponentSystemGroup
    {
    }
}