using Unity.Burst;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Server.Destroying
{
    [BurstCompile]
    public struct ServerNetworkEntityDestroyCommand : IRpcCommand
    {
        public uint networkEntityId;
    }
}