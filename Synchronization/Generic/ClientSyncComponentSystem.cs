using Plugins.ECSPowerNetcode.Client;
using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.UnityExtras.Logs;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Synchronization.Generic
{
    [UpdateInGroup(typeof(ClientRequestProcessingSystemGroup))]
    public abstract class ClientSyncComponentSystem<TComponent, TConverter> : ComponentSystem
        where TComponent : struct, IComponentData
        where TConverter : struct, ISyncEntityConverter<TComponent>
    {
        protected virtual bool ShouldApply(Entity entity, ref CopyEntityComponentRpcCommand<TComponent, TConverter> command)
        {
            return true;
        }

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref CopyEntityComponentRpcCommand<TComponent, TConverter> command, ref ReceiveRpcCommandRequestComponent requestComponent) =>
                {
                    var networkEntity = ClientManager.Instance.GetEntityByNetworkEntityId(command.networkEntityId);
                    if (networkEntity == Entity.Null)
                    {
                        UnityLogger.Warning($"Entity with networkEntityId {command.networkEntityId} doesn't exist");
                        return;
                    }

                    if (!ShouldApply(networkEntity, ref command))
                        return;

                    UnityLogger.Log($"Updated! {command.component.Value}");
                    PostUpdateCommands.SetComponent(networkEntity, command.component.Value);
                    PostUpdateCommands.DestroyEntity(entity);
                });
        }
    }
}