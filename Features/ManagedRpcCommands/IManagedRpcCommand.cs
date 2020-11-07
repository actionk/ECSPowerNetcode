using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Features.ManagedRpcCommands
{
    public interface IManagedRpcCommand : IRpcCommand
    {
        ulong PacketId { get; set; }
    }
}