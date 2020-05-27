using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.ECSPowerNetcode.Features.Packets;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Packets
{
    [UpdateInGroup(typeof(ClientRequestProcessingSystemGroup))]
    public abstract class AClientReceiveRpcCommandSystem<T> : AReceiveRpcCommandSystem<T> where T : struct, IRpcCommand
    {
    }
}