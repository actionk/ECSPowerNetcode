using Plugins.ECSPowerNetcode.Client.Packets;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Server.Packets;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    [UpdateInGroup(typeof(ServerRequestProcessingSystemGroup))]
    public abstract class AServerNetworkEntityBuilderSystem<TSelector, TCommand> : ComponentSystem
        where TSelector : struct, IComponentData
        where TCommand : struct, INetworkEntityCopyRpcCommand
    {
        protected abstract TCommand CreateTransferCommandForEntity(Entity entity, ref NetworkEntity networkEntity, ref TSelector selectorComponent);

        protected override void OnUpdate()
        {
            Entities
                .WithAll<TSelector, NetworkEntity, TransferNetworkEntityToAllClients>()
                .ForEach((Entity entity, ref NetworkEntity networkEntity, ref TSelector selectorComponent) =>
                {
                    var command = CreateTransferCommandForEntity(entity, ref networkEntity, ref selectorComponent);
                    ServerToClientRpcCommandBuilder
                        .Broadcast(command)
                        .Build(PostUpdateCommands);

                    PostUpdateCommands.RemoveComponent<TransferNetworkEntityToAllClients>(entity);
                });

            Entities
                .WithAll<TSelector, NetworkEntity, TransferNetworkEntityToClient>()
                .ForEach((Entity entity, DynamicBuffer<TransferNetworkEntityToClient> clients, ref NetworkEntity networkEntity, ref TSelector selectorComponent) =>
                {
                    var command = CreateTransferCommandForEntity(entity, ref networkEntity, ref selectorComponent);
                    foreach (var clientEntity in clients)
                    {
                        ServerToClientRpcCommandBuilder
                            .SendTo(clientEntity.clientConnection, command)
                            .Build(PostUpdateCommands);
                    }

                    PostUpdateCommands.RemoveComponent<TransferNetworkEntityToClient>(entity);
                });
        }
    }
}