using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Plugins.Shared.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class ClientConnectionSystemGroup : ComponentSystemGroup
    {
    }
}