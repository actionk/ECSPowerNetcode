using Plugins.ECSPowerNetcode.Client;
using Plugins.ECSPowerNetcode.EntityBulderExtensions;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Features.ManagedRpcCommands
{
    public class ClientToServerManagedRpcCommandBuilder : NetcodeEntityBuilder
    {
        private readonly ulong m_packetId;

        public static ClientToServerManagedRpcCommandBuilder Send<T>(T command) where T : struct, IManagedRpcCommand, IRpcCommand
        {
            var packetId = ClientManager.Instance.NextManagedPacketId;
            command.PacketId = packetId;

            var builder = new ClientToServerManagedRpcCommandBuilder(packetId);
            builder.AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = ClientManager.Instance.ConnectionToServer.connectionEntity})
                .SetName($"ManagedRpcCommand {typeof(T).Name}");

            return builder;
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