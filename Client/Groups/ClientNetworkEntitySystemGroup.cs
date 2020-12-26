using Plugins.Shared.Netcode.Entities.Groups.Client;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Client.Groups
{
    [UpdateInGroup(typeof(ClientInitializationSystemGroup))]
    public class ClientNetworkEntitySystemGroup : ComponentSystemGroup
    {
    }
}