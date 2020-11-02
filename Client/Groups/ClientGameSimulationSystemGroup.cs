using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Plugins.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class ClientGameSimulationSystemGroup : ComponentSystemGroup
    {
    }
}