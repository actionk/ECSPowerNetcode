using Plugins.ECSPowerNetcode.Client.Packets;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Server.Packets;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    [UpdateInGroup(typeof(ServerRequestProcessingSystemGroup))]
    public abstract class AServerNetworkEntityTransferSystem<TSelector, TCommand> : ComponentSystem
        where TSelector : struct, IComponentData
        where TCommand : struct, INetworkEntityCopyRpcCommand
    {
        protected abstract TCommand CreateTransferCommandForEntity(Entity entity, NetworkEntity networkEntity, TSelector selectorComponent);

        protected override void OnUpdate()
        {
            Entities
                .WithAll<TSelector, NetworkEntity, TransferNetworkEntityToAllClients>()
                .ForEach((Entity entity, ref NetworkEntity networkEntity, ref TSelector selectorComponent) =>
                {
                    var command = CreateTransferCommandForEntity(entity, networkEntity, selectorComponent);
                    ServerToClientRpcCommandBuilder.Broadcast()
                        .AddCommand(command)
                        .Build(PostUpdateCommands);

                    PostUpdateCommands.RemoveComponent<TransferNetworkEntityToAllClients>(entity);
                });

            Entities
                .WithAll<TSelector, NetworkEntity, TransferNetworkEntityToClient>()
                .ForEach((Entity entity, DynamicBuffer<TransferNetworkEntityToClient> clients, ref NetworkEntity networkEntity, ref TSelector selectorComponent) =>
                {
                    var command = CreateTransferCommandForEntity(entity, networkEntity, selectorComponent);
                    foreach (var clientEntity in clients)
                    {
                        ServerToClientRpcCommandBuilder.SendTo(clientEntity.clientConnection)
                            .AddCommand(command)
                            .Build(PostUpdateCommands);
                    }

                    PostUpdateCommands.RemoveComponent<TransferNetworkEntityToClient>(entity);
                });
        }
    }
}