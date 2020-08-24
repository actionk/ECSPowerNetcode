using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Plugins.ECSPowerNetcode.Features.Synchronization.Generic
{
    [BurstCompile]
    public struct CopyEntityComponentRpcCommand<TComponent, TConverter> : IComponentData, IRpcCommandSerializer<CopyEntityComponentRpcCommand<TComponent, TConverter>>
        where TConverter : struct, ISyncEntityConverter<TComponent>
    {
        public ulong networkEntityId;
        public TConverter component;

        #region Serialization

        public void Serialize(ref DataStreamWriter writer, in CopyEntityComponentRpcCommand<TComponent, TConverter> data)
        {
            writer.WriteULong(data.networkEntityId);
            data.component.Serialize(ref writer);
        }

        public void Deserialize(ref DataStreamReader reader, ref CopyEntityComponentRpcCommand<TComponent, TConverter> data)
        {
            data.networkEntityId = reader.ReadULong();
            data.component.Deserialize(ref reader);
        }

        #endregion

        #region Implementation

        public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute()
        {
            return INVOKE_EXECUTE_FUNCTION_POINTER;
        }

        [BurstCompile]
        private static void InvokeExecute(ref RpcExecutor.Parameters parameters)
        {
            RpcExecutor.ExecuteCreateRequestComponent<CopyEntityComponentRpcCommand<TComponent, TConverter>, CopyEntityComponentRpcCommand<TComponent, TConverter>>(ref parameters);
        }

        private static readonly PortableFunctionPointer<RpcExecutor.ExecuteDelegate> INVOKE_EXECUTE_FUNCTION_POINTER =
            new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);

        #endregion
    }
}