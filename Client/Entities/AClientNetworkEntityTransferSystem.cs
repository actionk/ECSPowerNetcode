using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.ECSPowerNetcode.Client.Packets;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Entities
{
    [UpdateInGroup(typeof(ClientRequestProcessingSystemGroup))]
    public abstract class AClientNetworkEntityTransferSystem<TCommand> : ComponentSystem
        where TCommand : struct, INetworkEntityCopyRpcCommand
    {
        protected abstract void CreateNetworkEntity(ulong networkEntityId, TCommand command);
        protected abstract void SynchronizeNetworkEntity(Entity entity, TCommand command);

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref TCommand command, ref ReceiveRpcCommandRequestComponent requestComponent) =>
                {
                    var existingEntity = ClientManager.Instance.GetEntityByNetworkEntityId(command.NetworkEntityId);
                    if (existingEntity != Entity.Null)
                        SynchronizeNetworkEntity(existingEntity, command);
                    else
                        CreateNetworkEntity(command.NetworkEntityId, command);

                    PostUpdateCommands.DestroyEntity(entity);
                });
        }
    }
}