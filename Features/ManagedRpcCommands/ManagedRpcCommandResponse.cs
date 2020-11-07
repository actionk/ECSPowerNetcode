using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Features.ManagedRpcCommands
{
    public struct ManagedRpcCommandResponse : IComponentData
    {
        public ulong packetId;
        public int result;
    }
}