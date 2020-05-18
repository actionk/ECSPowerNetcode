using Plugins.ECSEntityBuilder;
using Plugins.ECSEntityBuilder.Steps;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Server.Packets
{
    public class ServerToClientRpcCommandBuilder : EntityBuilder<ServerToClientRpcCommandBuilder>
    {
        protected uint excludedPlayerId;
        protected bool isBroadcast;
        protected override ServerToClientRpcCommandBuilder Self => this;

        public static ServerToClientRpcCommandBuilder SendTo<T>(Entity serverToClientConnection, T command) where T : struct, IRpcCommand
        {
            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = serverToClientConnection})
                .SetName($"RpcCommand {typeof(T).Name}");
        }

        public static ServerToClientRpcCommandBuilder SendTo(Entity serverToClientConnection)
        {
            return new ServerToClientRpcCommandBuilder()
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = serverToClientConnection})
                .SetName("RpcCommand");
        }

        public static ServerToClientRpcCommandBuilder Broadcast()
        {
            return new ServerToClientRpcCommandBuilder()
                .SetBroadcast(true)
                .SetName("BroadcastRpcCommand");
        }

        public ServerToClientRpcCommandBuilder SetBroadcast(bool isBroadcast)
        {
            this.isBroadcast = isBroadcast;
            return this;
        }

        public ServerToClientRpcCommandBuilder AddCommand<T>(T component) where T : struct, IRpcCommand
        {
            GetOrCreateGenericStep<AddComponentDataStep<T>, T>().SetValue(component);
            return this;
        }

        public override Entity Build(EntityManagerWrapper wrapper)
        {
            if (!isBroadcast)
                return base.Build(wrapper);

            var connections = ServerManager.Instance.AllConnections;
            foreach (var connection in connections)
            {
                var entity = base.Build(wrapper);
                wrapper.AddComponentData(entity, new SendRpcCommandRequestComponent {TargetConnection = connection});
            }

            return Entity.Null;
        }

        public ServerToClientRpcCommandBuilder ExcludePlayersConnection(uint playerId)
        {
            excludedPlayerId = playerId;
            return this;
        }
    }
}