using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Packets
{
    [UpdateInGroup(typeof(ServerRequestProcessingSystemGroup))]
    public abstract class AServerReceiveRpcCommandSystem<T> : ComponentSystem where T : struct, IComponentData
    {
        protected virtual bool ShouldDestroyEntity { get; } = true;

        protected abstract void OnCommand(ref T command, ConnectionDescription clientConnection);

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref T command, ref ReceiveRpcCommandRequestComponent requestComponent) =>
                {
                    var clientConnection = ServerManager.Instance.GetClientConnectionByConnectionEntity(requestComponent.SourceConnection);

                    if (ShouldDestroyEntity)
                        PostUpdateCommands.DestroyEntity(entity);

                    OnCommand(ref command, clientConnection);
                });
        }
    }
}