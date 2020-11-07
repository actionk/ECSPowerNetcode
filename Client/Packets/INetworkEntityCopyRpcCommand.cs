using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Client.Packets
{
    public interface INetworkEntityCopyRpcCommand : IComponentData
    {
        uint NetworkEntityId { get; }
    }
}