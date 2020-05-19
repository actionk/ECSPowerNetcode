using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Server.Exceptions;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Packets
{
    public class ServerToClientRpcCommandBuilder : EntityBuilder<ServerToClientRpcCommandBuilder>
    {
        protected override ServerToClientRpcCommandBuilder Self => this;

        public static ServerToClientRpcCommandBuilder SendTo<T>(Entity serverToClientConnection, T command) where T : struct, IRpcCommand
        {
            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = serverToClientConnection})
                .SetName($"RpcCommand {typeof(T).Name}");
        }

        public static ServerToClientRpcCommandBuilder SendTo<T>(int networkId, T command) where T : struct, IRpcCommand
        {
            var connection = ServerManager.Instance.GetClientConnectionByNetworkId(networkId);
            if (connection.IsEmpty)
                throw new ClientConnectionNotFoundException($"No connection with network id [{networkId}] found");

            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = connection.connectionEntity})
                .SetName($"RpcCommand {typeof(T).Name}");
        }

        public static ServerToClientRpcCommandBuilder Broadcast<T>(T command) where T : struct, IRpcCommand
        {
            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent()) // if there is no TargetConnection, it will be broadcasted
                .SetName($"BroadcastRpcCommand {typeof(T).Name}");
        }
    }
}