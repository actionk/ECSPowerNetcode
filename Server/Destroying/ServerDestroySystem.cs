using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Server.Groups;
using Plugins.ECSPowerNetcode.Server.Packets;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Destroying
{
    [UpdateInGroup(typeof(ServerCleanupSystemGroup))]
    public class ServerDestroySystem : ComponentSystem
    {
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

                    PostUpdateCommands.DestroyEntity(entity);
                });


            Entities
                .WithNone<NetworkEntity>()
                .ForEach((Entity entity, ref ServerDestroy destroy) => { PostUpdateCommands.DestroyEntity(entity); });
        }
    }
}