using System;
using Plugins.Shared.ECSPowerNetcode.Server.Exceptions;
using Plugins.Shared.ECSPowerNetcode.Server.Groups;
using Plugins.Shared.ECSPowerNetcode.Shared;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.Shared.ECSPowerNetcode.Server.Packets
{
    [UpdateInGroup(typeof(ServerRequestProcessingSystemGroup))]
    public abstract class AServerReceiveRpcCommandSystem<T> : ComponentSystem where T : struct, IComponentData
    {
        protected virtual bool ShouldDestroyEntity { get; } = true;

        protected abstract void OnCommand(ref T packet, ConnectionDescription clientConnection);

        protected override void OnUpdate()
        {
            Entities
                .ForEach((Entity entity, ref T command, ref ReceiveRpcCommandRequestComponent requestComponent) =>
                {
                    var clientConnection = ServerManager.Instance.GetClientConnectionByConnectionEntity(requestComponent.SourceConnection);

                    if (ShouldDestroyEntity)
                        PostUpdateCommands.DestroyEntity(entity);

                    try
                    {
                        OnCommand(ref command, clientConnection);
                    }
                    catch (ServerException e)
                    {
                        Debug.LogWarning($"{GetType()} Expected server error: {e.Message}");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"{GetType()} Unexpected server error: {e.Message}");
                        Debug.LogException(e);
                    }
                });
        }
    }
}