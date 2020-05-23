using Plugins.ECSPowerNetcode.Client.Packets;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Server.Packets;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    [UpdateInGroup(typeof(ServerRequestProcessingSystemGroup))]
    public abstract class AServerNetworkEntityBuilderSystemT2<TSelector, TSelector2, TCommand> : ComponentSystem
        where TSelector : struct, IComponentData
        where TSelector2 : struct, IComponentData
        where TCommand : struct, INetworkEntityCopyRpcCommand
    {
        protected abstract TCommand CreateTransferCommandForEntity(Entity entity, NetworkEntity networkEntity, ref TSelector selectorComponent, ref TSelector2 selectorComponent2);

        protected override void OnUpdate()
        {
            Entities
                .WithAll<TSelector, TSelector2, NetworkEntity, TransferNetworkEntityToAllClients>()
                .ForEach((Entity entity, ref NetworkEntity networkEntity, ref TSelector selectorComponent, ref TSelector2 selectorComponent2) =>
                {
                    var command = CreateTransferCommandForEntity(entity, networkEntity, ref selectorComponent, ref selectorComponent2);
                    ServerToClientRpcCommandBuilder
                        .Broadcast(command)
                        .Build(PostUpdateCommands);

                    PostUpdateCommands.RemoveComponent<TransferNetworkEntityToAllClients>(entity);
                });

            Entities
                .WithAll<TSelector, NetworkEntity, TransferNetworkEntityToClient>()
                .ForEach((Entity entity, DynamicBuffer<TransferNetworkEntityToClient> clients, ref NetworkEntity networkEntity, ref TSelector selectorComponent, ref TSelector2 selectorComponent2) =>
                {
                    var command = CreateTransferCommandForEntity(entity, networkEntity, ref selectorComponent, ref selectorComponent2);
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