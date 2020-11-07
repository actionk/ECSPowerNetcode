using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Client.Packets
{
    public interface INetworkEntityCopyRpcCommand : IComponentData
    {
        uint NetworkEntityId { get; }
    }
}