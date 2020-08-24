using Plugins.ECSEntityBuilder;
using Plugins.ECSPowerNetcode.Server.Exceptions;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Packets
{
    public class ServerToClientRpcCommandBuilder : EntityBuilder<ServerToClientRpcCommandBuilder>
    {
        public static ServerToClientRpcCommandBuilder SendTo<T>(Entity serverToClientConnection, T command) where T : struct, IComponentData
        {
            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = serverToClientConnection})
                .SetName($"RpcCommand {typeof(T).Name}");
        }

        public static ServerToClientRpcCommandBuilder SendTo<T>(int networkConnectionId, T command) where T : struct, IComponentData
        {
            var connection = ServerManager.Instance.GetClientConnectionByNetworkId(networkConnectionId);
            if (connection.IsEmpty)
                throw new ClientConnectionNotFoundException($"No connection with network id [{networkConnectionId}] found");

            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = connection.connectionEntity})
                .SetName($"RpcCommand {typeof(T).Name}");
        }

        public static ServerToClientRpcCommandBuilder Broadcast<T>(T command) where T : struct, IComponentData
        {
            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent()) // if there is no TargetConnection, it will be broadcasted
                .SetName($"BroadcastRpcCommand {typeof(T).Name}");
        }
    }
}