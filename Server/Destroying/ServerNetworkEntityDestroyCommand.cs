using Unity.Burst;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Destroying
{
    [BurstCompile]
    public struct ServerNetworkEntityDestroyCommand : IRpcCommand
    {
        public ulong networkEntityId;
    }
}