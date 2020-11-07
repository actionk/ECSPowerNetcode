using Plugins.Shared.ECSPowerNetcode.Client;
using Plugins.Shared.ECSPowerNetcode.Client.Groups;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.Shared.ECSPowerNetcode.Features.Synchronization.Generic
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
                    var networkEntity = ClientManager.Instance.NetworkEntityManager.TryGetEntityByNetworkEntityId(command.networkEntityId);
                    if (networkEntity == Entity.Null)
                    {
                        Debug.LogWarning($"Entity with networkEntityId {command.networkEntityId} doesn't exist");
                        return;
                    }

                    PostUpdateCommands.DestroyEntity(entity);

                    if (!ShouldApply(networkEntity, ref command))
                        return;

                    PostUpdateCommands.SetComponent(networkEntity, command.component.Value);
                });
        }
    }
}