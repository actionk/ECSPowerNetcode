using System.Linq;
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

        public static ServerToClientRpcCommandBuilder SendTo<T>(int networkConnectionId, T packet) where T : struct, IComponentData
        {
            var connection = ServerManager.Instance.GetClientConnectionByNetworkId(networkConnectionId);
            if (connection.IsEmpty)
                throw new ClientConnectionNotFoundException($"No connection with network id [{networkConnectionId}] found");

            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(packet)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = connection.connectionEntity})
                .SetName($"RpcCommand {typeof(T).Name}");
        }

        public static ServerToClientRpcCommandBuilder Broadcast<T>(T packet) where T : struct, IComponentData
        {
            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(packet)
                .AddComponentData(new SendRpcCommandRequestComponent()) // if there is no TargetConnection, it will be broadcasted
                .SetName($"BroadcastRpcCommand {typeof(T).Name}");
        }

        public static ServerToClientMassiveRpcCommandBuilder SendToAllConnectionsExcept<T>(T packet, Entity excludeNetworkConnection) where T : struct, IComponentData
        {
            var connectionsToSendTo = ServerManager.Instance.AllConnections
                .FindAll(x => x.connectionEntity != excludeNetworkConnection)
                .Select(x => x.connectionEntity)
                .ToArray();

            return new ServerToClientMassiveRpcCommandBuilder(connectionsToSendTo)
                .AddComponentData(packet)
                .SetName($"RpcCommand {typeof(T).Name}");
        }
    }

    public class ServerToClientMassiveRpcCommandBuilder : EntityBuilder<ServerToClientMassiveRpcCommandBuilder>
    {
        private readonly Entity[] connections;

        internal ServerToClientMassiveRpcCommandBuilder(Entity[] connections)
        {
            this.connections = connections;
        }

        public override Entity Build(EntityManagerWrapper wrapper)
        {
            foreach (var connection in connections)
            {
                AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = connection});
                base.Build(wrapper);
            }

            return Entity.Null;
        }
    }
}