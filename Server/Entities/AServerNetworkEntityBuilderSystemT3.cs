using Plugins.ECSPowerNetcode.Client.Packets;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Server.Packets;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Entities
{
    [UpdateInGroup(typeof(ServerRequestProcessingSystemGroup))]
    [UpdateInWorld(UpdateInWorld.TargetWorld.Server)]
    public abstract class AServerNetworkEntityBuilderSystemT3<TSelector, TSelector2, TSelector3, TCommand> : ComponentSystem
        where TSelector : struct, IComponentData
        where TSelector2 : struct, IComponentData
        where TSelector3 : struct, IComponentData
        where TCommand : struct, INetworkEntityCopyRpcCommand
    {
        protected abstract TCommand CreateTransferCommandForEntity(Entity entity, ref NetworkEntity networkEntity, ref TSelector selectorComponent,
            ref TSelector2 selectorComponent2,
            ref TSelector3 selectorComponent3);

        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkEntity, NetworkEntityRegistered, TransferNetworkEntityToAllClients>()
                .ForEach((Entity entity, ref NetworkEntity networkEntity, ref TSelector selectorComponent, ref TSelector2 selectorComponent2, ref TSelector3 selectorComponent3) =>
                {
                    if (ServerManager.Instance.HasConnections)
                    {
                        var command = CreateTransferCommandForEntity(entity, ref networkEntity, ref selectorComponent, ref selectorComponent2, ref selectorComponent3);
                        ServerToClientRpcCommandBuilder
                            .Broadcast(command)
                            .Build(PostUpdateCommands);
                    }

                    PostUpdateCommands.RemoveComponent<TransferNetworkEntityToAllClients>(entity);
                });

            Entities
                .WithAll<NetworkEntity, NetworkEntityRegistered, TransferNetworkEntityToClient>()
                .ForEach((Entity entity, DynamicBuffer<TransferNetworkEntityToClient> clients, ref NetworkEntity networkEntity, ref TSelector selectorComponent,
                    ref TSelector2 selectorComponent2, ref TSelector3 selectorComponent3) =>
                {
                    if (ServerManager.Instance.HasConnections)
                    {
                        var command = CreateTransferCommandForEntity(entity, ref networkEntity, ref selectorComponent, ref selectorComponent2, ref selectorComponent3);
                        foreach (var clientEntity in clients)
                        {
                            ServerToClientRpcCommandBuilder
                                .SendTo(clientEntity.clientConnection, command)
                                .Build(PostUpdateCommands);
                        }
                    }

                    PostUpdateCommands.RemoveComponent<TransferNetworkEntityToClient>(entity);
                });
        }
    }
}