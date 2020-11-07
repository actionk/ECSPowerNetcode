using Unity.Entities;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientInitializationSystemGroup))]
    public class ClientNetworkEntitySystemGroup : ComponentSystemGroup
    {
    }
}