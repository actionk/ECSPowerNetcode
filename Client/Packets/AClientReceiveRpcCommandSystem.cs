using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Packets
{
    [UpdateInGroup(typeof(ClientRequestProcessingSystemGroup))]
    public abstract class AClientReceiveRpcCommandSystem<T> : ComponentSystem where T : struct, IComponentData
    {
        protected virtual bool ShouldDestroyEntity { get; } = true;

        protected abstract void OnCommand(ref T packet, ConnectionDescription connectionToServer);

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref T command, ref ReceiveRpcCommandRequestComponent requestComponent) =>
                {
                    var connectionDescription = ClientManager.Instance.ConnectionToServer;

                    if (ShouldDestroyEntity)
                        PostUpdateCommands.DestroyEntity(entity);

                    OnCommand(ref command, connectionDescription);
                });
        }
    }
}