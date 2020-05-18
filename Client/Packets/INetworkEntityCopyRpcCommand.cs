using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Packets
{
    public interface INetworkEntityCopyRpcCommand : IRpcCommand
    {
        ulong NetworkEntityId { get; }
    }
}