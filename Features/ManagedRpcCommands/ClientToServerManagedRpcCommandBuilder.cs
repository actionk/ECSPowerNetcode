using Plugins.Shared.ECSEntityBuilder;
using Plugins.Shared.ECSPowerNetcode.Client;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Features.ManagedRpcCommands
{
    public class ClientToServerManagedRpcCommandBuilder : EntityBuilder<ClientToServerManagedRpcCommandBuilder>
    {
        private readonly ulong m_packetId;

        public static ClientToServerManagedRpcCommandBuilder Send<T>(T command) where T : struct, IManagedRpcCommand, IRpcCommand
        {
            var packetId = ClientManager.Instance.NextManagedPacketId;
            command.PacketId = packetId;
            return new ClientToServerManagedRpcCommandBuilder(packetId)
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = ClientManager.Instance.ConnectionToServer.connectionEntity})
                .SetName($"ManagedRpcCommand {typeof(T).Name}");
        }

        public ClientToServerManagedRpcCommandBuilder(ulong packetId)
        {
            m_packetId = packetId;
        }

        public void AddEntityWaitingForResult(Entity entity)
        {
            ClientManager.Instance.AddEntityWaitingForManagedRpcCommandResult(entity, m_packetId);
        }
    }
}