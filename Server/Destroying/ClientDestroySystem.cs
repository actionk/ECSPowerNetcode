using Plugins.ECSPowerNetcode.Client;
using Plugins.ECSPowerNetcode.Client.Packets;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Destroying
{
    public class ClientDestroySystem : AClientReceiveRpcCommandSystem<ServerNetworkEntityDestroyCommand>
    {
        protected override void OnCommand(ref ServerNetworkEntityDestroyCommand command, ref ReceiveRpcCommandRequestComponent requestComponent)
        {
            var networkEntity = ClientManager.Instance.GetEntityByNetworkEntityId(command.networkEntityId);
            if (networkEntity == Entity.Null)
                return;
            
            PostUpdateCommands.DestroyEntity(networkEntity);
        }
    }
}