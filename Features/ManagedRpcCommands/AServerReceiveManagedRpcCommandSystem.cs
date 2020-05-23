using Plugins.ECSPowerNetcode.Server.Packets;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Features.ManagedRpcCommands
{
    public abstract class AServerReceiveManagedRpcCommandSystem<T> : ComponentSystem where T : struct, IManagedRpcCommand, IRpcCommand
    {
        protected virtual bool ShouldDestroyEntity { get; } = true;

        protected abstract int OnCommand(ref T command, ref ReceiveRpcCommandRequestComponent requestComponent);

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref T command, ref ReceiveRpcCommandRequestComponent requestComponent) =>
                {
                    if (ShouldDestroyEntity)
                        PostUpdateCommands.DestroyEntity(entity);

                    int result = OnCommand(ref command, ref requestComponent);
                    ServerToClientRpcCommandBuilder
                        .SendTo(requestComponent.SourceConnection, new ManagedRpcCommandResult
                        {
                            packetId = command.PacketId,
                            result = result
                        })
                        .Build(PostUpdateCommands);
                });
        }
    }
}