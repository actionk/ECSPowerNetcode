using Plugins.ECSEntityBuilder;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Client.Packets
{
    public class ClientToServerRpcCommandBuilder : EntityBuilder<ClientToServerRpcCommandBuilder>
    {
        protected override ClientToServerRpcCommandBuilder Self => this;

        public static ClientToServerRpcCommandBuilder Create<T>(T command) where T : struct, IRpcCommand
        {
            return new ClientToServerRpcCommandBuilder()
                .AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = ClientManager.Instance.ConnectionToServer.connectionEntity})
                .SetName($"RpcCommand {typeof(T).Name}");
        }
    }
}