using Plugins.Shared.ECSPowerNetcode.EntityBulderExtensions;
using Unity.Entities;
using Unity.NetCode;

namespace Plugins.Shared.ECSPowerNetcode.Client.Packets
{
    public class ClientToServerRpcCommandBuilder : NetcodeEntityBuilder
    {
        public static ClientToServerRpcCommandBuilder Send<T>(T command) where T : struct, IComponentData
        {
            var builder = new ClientToServerRpcCommandBuilder();
            builder.AddComponentData(command)
                .AddComponentData(new SendRpcCommandRequestComponent {TargetConnection = ClientManager.Instance.ConnectionToServer.connectionEntity})
                .SetName($"RpcCommand {typeof(T).Name}");

            return builder;
        }
    }
}