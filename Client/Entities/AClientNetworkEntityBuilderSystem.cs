using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.ECSPowerNetcode.Client.Packets;
using Plugins.UnityExtras.Logs;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Entities
{
    [UpdateInGroup(typeof(ClientRequestProcessingSystemGroup))]
    public abstract class AClientNetworkEntityBuilderSystem<TCommand> : ComponentSystem
        where TCommand : struct, INetworkEntityCopyRpcCommand
    {
        protected abstract void CreateNetworkEntity(ulong networkEntityId, TCommand command);
        protected abstract void SynchronizeNetworkEntity(Entity entity, TCommand command);

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref TCommand command, ref ReceiveRpcCommandRequestComponent requestComponent) =>
                {
                    UnityLogger.Log($"[Client] Creating a client entity with id {command.NetworkEntityId}");

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