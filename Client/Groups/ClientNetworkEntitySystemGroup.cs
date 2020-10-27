using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Plugins.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientInitializationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class ClientNetworkEntitySystemGroup : ComponentSystemGroup
    {
    }
}