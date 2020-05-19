using Plugins.ECSPowerNetcode.Client;
using Plugins.UnityExtras.Logs;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Synchronization.Transform
{
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class ClientSyncTransformSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref SyncTransformFromServerToClientCommand snapshot, ref ReceiveRpcCommandRequestComponent reqSrc) =>
                {
                    var modifiedEntity = ClientManager.Instance.GetEntityByNetworkEntityId(snapshot.networkEntityId);
                    if (modifiedEntity == Entity.Null)
                    {
                        Debug.LogWarning($"Entity with networkEntityId {snapshot.networkEntityId} doesn't exist");
                        return;
                    }

                    if (EntityManager.HasComponent<IgnoreTransformCopyingFromServer>(modifiedEntity))
                        return;

                    PostUpdateCommands.SetComponent(modifiedEntity, new Translation {Value = snapshot.position});
                    PostUpdateCommands.SetComponent(modifiedEntity, new Rotation {Value = snapshot.rotation});
                    PostUpdateCommands.SetComponent(modifiedEntity, new Scale {Value = snapshot.scale});

                    PostUpdateCommands.DestroyEntity(entity);
                });
        }
    }
}