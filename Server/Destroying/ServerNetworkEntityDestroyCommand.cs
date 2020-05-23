using Unity.Burst;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Plugins.ECSPowerNetcode.Server.Destroying
{
    [BurstCompile]
    public struct ServerNetworkEntityDestroyCommand : IRpcCommand
    {
        public ulong networkEntityId;

        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteULong(networkEntityId);
        }

        public void Deserialize(ref DataStreamReader reader)
        {
            networkEntityId = reader.ReadULong();
        }

        #region Implementation

        public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute()
        {
            return INVOKE_EXECUTE_FUNCTION_POINTER;
        }

        [BurstCompile]
        private static void InvokeExecute(ref RpcExecutor.Parameters parameters)
        {
            RpcExecutor.ExecuteCreateRequestComponent<ServerNetworkEntityDestroyCommand>(ref parameters);
        }

        private static readonly PortableFunctionPointer<RpcExecutor.ExecuteDelegate> INVOKE_EXECUTE_FUNCTION_POINTER =
            new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);

        #endregion

        #region Sender

        public class ServerNetworkEntityDestroyCommandSender : RpcCommandRequestSystem<ServerNetworkEntityDestroyCommand>
        {
        }

        #endregion
    }
}