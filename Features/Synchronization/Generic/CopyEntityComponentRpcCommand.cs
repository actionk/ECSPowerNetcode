using Unity.Burst;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Plugins.ECSPowerNetcode.Features.Synchronization.Generic
{
    [BurstCompile]
    public struct CopyEntityComponentRpcCommand<TComponent, TConverter> : IRpcCommand
        where TConverter : struct, ISyncEntityConverter<TComponent>
    {
        public ulong networkEntityId;
        public TConverter component;

        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteULong(networkEntityId);
            component.Serialize(ref writer);
        }

        public void Deserialize(ref DataStreamReader reader)
        {
            networkEntityId = reader.ReadULong();
            component.Deserialize(ref reader);
        }

        #region Implementation

        public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute()
        {
            return INVOKE_EXECUTE_FUNCTION_POINTER;
        }

        [BurstCompile]
        private static void InvokeExecute(ref RpcExecutor.Parameters parameters)
        {
            RpcExecutor.ExecuteCreateRequestComponent<CopyEntityComponentRpcCommand<TComponent, TConverter>>(ref parameters);
        }

        private static readonly PortableFunctionPointer<RpcExecutor.ExecuteDelegate> INVOKE_EXECUTE_FUNCTION_POINTER =
            new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);

        #endregion
    }
}