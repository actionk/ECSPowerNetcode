using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientInitializationSystemGroup))]
    public class ClientNetworkEntitySystemGroup : ComponentSystemGroup
    {
    }
}