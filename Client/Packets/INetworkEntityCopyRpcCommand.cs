using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Packets
{
    public interface INetworkEntityCopyRpcCommand : IComponentData
    {
        ulong NetworkEntityId { get; }
    }
}