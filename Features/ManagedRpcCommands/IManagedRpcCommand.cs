using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Features.ManagedRpcCommands
{
    public interface IManagedRpcCommand : IRpcCommand
    {
        ulong PacketId { get; set; }
    }
}