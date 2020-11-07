using Unity.Burst;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Features.ManagedRpcCommands
{
    [BurstCompile]
    public struct ManagedRpcCommandResult : IRpcCommand
    {
        public ulong packetId;
        public int result;
    }
}