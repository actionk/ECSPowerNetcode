using Plugins.ECSPowerNetcode.Server.Groups;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Features.Packets
{
    [UpdateInGroup(typeof(ServerRequestProcessingSystemGroup))]
    public abstract class AServerReceiveRpcCommandSystem<T> : AReceiveRpcCommandSystem<T> where T : struct, IComponentData
    {
    }
}