using Plugins.ECSPowerNetcode.Client;
using Plugins.ECSPowerNetcode.Client.Packets;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Destroying
{
    public class ClientDestroySystem : AClientReceiveRpcCommandSystem<ServerNetworkEntityDestroyCommand>
    {
        protected override void OnCommand(ref ServerNetworkEntityDestroyCommand packet, ConnectionDescription connectionToServer)
        {
            var networkEntity = ClientManager.Instance.NetworkEntityManager.TryGetEntityByNetworkEntityId(packet.networkEntityId);
            if (networkEntity == Entity.Null)
                return;

            PostUpdateCommands.DestroyEntity(networkEntity);
        }
    }
}