using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Features.ManagedRpcCommands
{
    public struct ManagedRpcCommandResponse : IComponentData
    {
        public ulong packetId;
        public int result;
    }
}