using Plugins.Shared.ECSEntityBuilder;
using Plugins.Shared.ECSPowerNetcode.Server.Components;
using Plugins.Shared.ECSPowerNetcode.Server.Groups;
using Plugins.Shared.ECSPowerNetcode.Shared.Components;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.Shared.ECSPowerNetcode.Server.Lifecycle
{
    [UpdateInGroup(typeof(ServerConnectionSystemGroup))]
    [UpdateAfter(typeof(ServerStartSystem))]
    [UpdateInWorld(UpdateInWorld.TargetWorld.Server)]
    public class ServerNewClientConnectedSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .WithAll<NetworkIdComponent>()
                .WithNone<NetworkStreamInGame>()
                .ForEach((Entity connectionEntity, ref NetworkIdComponent networkIdComponent) =>
                {
                    Debug.Log($"[Server] Client connected with network id = [{networkIdComponent.Value}]");

                    EntityWrapper.Wrap(connectionEntity, EntityManager)
                        .SetName($"ClientConnection_{networkIdComponent.Value}");

                    var connectionCommandHandler = EntityWrapper.CreateEntity(EntityManager)
                        .AddComponentData(new ServerToClientCommandHandler {connectionEntity = connectionEntity})
                        .SetName($"ClientConnection_{networkIdComponent.Value}_CommandBuffer")
                        .Entity;

                    PostUpdateCommands.SetComponent(connectionEntity, new CommandTargetComponent {targetEntity = connectionCommandHandler});
                    PostUpdateCommands.AddComponent<NetworkStreamInGame>(connectionEntity);

                    ServerManager.Instance.OnConnected(networkIdComponent.Value, connectionEntity, connectionCommandHandler);

                    var ghostCollection = GetSingletonEntity<GhostPrefabCollectionComponent>();
                    var prefab = Entity.Null;
                    var prefabs = EntityManager.GetBuffer<GhostPrefabBuffer>(ghostCollection);
                    for (int ghostId = 0; ghostId < prefabs.Length; ++ghostId)
                    {
                        if (EntityManager.HasComponent<SyncGhostComponent>(prefabs[ghostId].Value))
                            prefab = prefabs[ghostId].Value;
                    }

                    EntityWrapper.Instantiate(prefab, PostUpdateCommands)
                        .SetName($"ClientConnection_{networkIdComponent.Value}_Ghost")
                        .AddComponentData(new GhostOwnerComponent {NetworkId = networkIdComponent.Value});
                });
        }
    }
}