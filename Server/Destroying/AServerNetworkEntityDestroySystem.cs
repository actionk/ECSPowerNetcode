using Plugins.Shared.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.Shared.ECSPowerNetcode.Server.Groups;
using Plugins.Shared.ECSPowerNetcode.Server.Packets;
using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Server.Destroying
{
    [UpdateInGroup(typeof(ServerCleanupSystemGroup))]
    public abstract class AServerNetworkEntityDestroySystem : ComponentSystem
    {
        protected abstract void OnDestroyEntity(uint networkEntityId, Entity entity);

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref NetworkEntity networkEntity, ref ServerDestroy destroy) =>
                {
                    ServerToClientRpcCommandBuilder
                        .Broadcast(new ServerNetworkEntityDestroyCommand
                        {
                            networkEntityId = networkEntity.networkEntityId
                        })
                        .Build(PostUpdateCommands);

                    OnDestroyEntity(networkEntity.networkEntityId, entity);
                });


            Entities
                .WithNone<NetworkEntity>()
                .ForEach((Entity entity, ref ServerDestroy destroy) => { PostUpdateCommands.DestroyEntity(entity); });
        }
    }
}