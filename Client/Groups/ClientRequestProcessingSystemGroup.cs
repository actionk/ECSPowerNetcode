using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Plugins.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    [UpdateAfter(typeof(ClientConnectionSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class ClientRequestProcessingSystemGroup : ComponentSystemGroup
    {
    }
}