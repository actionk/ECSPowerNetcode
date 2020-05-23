using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.ECSPowerNetcode.Features.Packets;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Packets
{
    [UpdateInGroup(typeof(ClientRequestProcessingSystemGroup))]
    public abstract class AClientReceiveRpcCommandSystem<T> : AReceiveRpcCommandSystem<T> where T : struct, IRpcCommand
    {
    }
}