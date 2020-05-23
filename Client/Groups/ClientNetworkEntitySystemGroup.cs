using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Plugins.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    [UpdateAfter(typeof(ClientRequestProcessingSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class ClientNetworkEntitySystemGroup : ComponentSystemGroup
    {
    }
}