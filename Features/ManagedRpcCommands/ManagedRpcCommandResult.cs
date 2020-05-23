using Unity.Burst;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Plugins.ECSPowerNetcode.Features.ManagedRpcCommands
{
    [BurstCompile]
    public struct ManagedRpcCommandResult : IRpcCommand
    {
        public ulong packetId;
        public int result;

        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteULong(packetId);
            writer.WriteInt(result);
        }

        public void Deserialize(ref DataStreamReader reader)
        {
            packetId = reader.ReadULong();
            result = reader.ReadInt();
        }

        #region Implementation

        public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute()
        {
            return INVOKE_EXECUTE_FUNCTION_POINTER;
        }

        [BurstCompile]
        private static void InvokeExecute(ref RpcExecutor.Parameters parameters)
        {
            RpcExecutor.ExecuteCreateRequestComponent<ManagedRpcCommandResult>(ref parameters);
        }

        private static readonly PortableFunctionPointer<RpcExecutor.ExecuteDelegate> INVOKE_EXECUTE_FUNCTION_POINTER =
            new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);

        #endregion

        #region Sender

        public class RpcCommandResultSender : RpcCommandRequestSystem<ManagedRpcCommandResult>
        {
        }

        #endregion
    }
}