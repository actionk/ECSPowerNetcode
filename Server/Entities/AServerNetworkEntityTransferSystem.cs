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
        }
    }
}