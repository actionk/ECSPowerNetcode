using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.ECSPowerNetcode.Features.Packets;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Client.Packets
{
    [UpdateInGroup(typeof(ClientRequestProcessingSystemGroup))]
    public abstract class AClientReceiveRpcCommandSystem<T> : AReceiveRpcCommandSystem<T> where T : struct, IComponentData
    {
    }
}