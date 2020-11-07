using Plugins.ECSPowerNetcode.Client.Packets;
using Plugins.ECSPowerNetcode.Server.Destroying;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Entities
{
    [UpdateInWorld(UpdateInWorld.TargetWorld.Client)]
    public abstract class AClientNetworkEntityDestroySystem : AClientReceiveRpcCommandSystem<ServerNetworkEntityDestroyCommand>
    {
        protected abstract void OnDestroyEntity(uint networkEntityId, Entity entity);

        protected override void OnCommand(ref ServerNetworkEntityDestroyCommand packet, ConnectionDescription connectionToServer)
        {
            var networkEntity = ClientManager.Instance.NetworkEntityManager.TryGetEntityByNetworkEntityId(packet.networkEntityId);
            if (networkEntity == Entity.Null)
                return;

            OnDestroyEntity(packet.networkEntityId, networkEntity);
        }
    }
}