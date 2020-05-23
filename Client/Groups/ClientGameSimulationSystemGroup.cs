using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Plugins.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    [UpdateAfter(typeof(ClientNetworkEntitySystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class ClientGameSimulationSystemGroup : ComponentSystemGroup
    {
    }
}