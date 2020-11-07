using Plugins.Shared.ECSEntityBuilder;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Client.Packets
{
    public class ClientToServerRpcCommandBuilder : EntityBuilder<ClientToServerRpcCommandBuilder>
    {
        public static ClientToServerRpcCommandBuilder Send<T>(T command) where T : struct, IComponentData
        {
            return new ClientToServerRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = ClientManager.Instance.ConnectionToServer.connectionEntity})
                .SetName($"RpcCommand {typeof(T).Name}");
        }
    }
}