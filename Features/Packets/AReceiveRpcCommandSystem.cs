using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Features.Packets
{
    public abstract class AReceiveRpcCommandSystem<T> : ComponentSystem where T : struct, IComponentData
    {
        protected virtual bool ShouldDestroyEntity { get; } = true;

        protected abstract void OnCommand(ref T command, ref ReceiveRpcCommandRequestComponent requestComponent);

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref T command, ref ReceiveRpcCommandRequestComponent requestComponent) =>
                {
                    if (ShouldDestroyEntity)
                        PostUpdateCommands.DestroyEntity(entity);

                    OnCommand(ref command, ref requestComponent);
                });
        }
    }
}