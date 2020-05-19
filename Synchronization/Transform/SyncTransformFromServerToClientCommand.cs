using Unity.Burst;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Plugins.ECSPowerNetcode.Synchronization.Transform
{
    [BurstCompile]
    public struct SyncTransformFromServerToClientCommand : IRpcCommand
    {
        public uint tick;
        public ulong networkEntityId;
        public float3 position;
        public quaternion rotation;
        public float scale;

        public void Serialize(ref DataStreamWriter writer)
        {
            writer.WriteUInt(tick);
            writer.WriteULong(networkEntityId);

            writer.WriteFloat(position.x);
            writer.WriteFloat(position.y);
            writer.WriteFloat(position.z);

            writer.WriteFloat(rotation.value.x);
            writer.WriteFloat(rotation.value.y);
            writer.WriteFloat(rotation.value.z);
            writer.WriteFloat(rotation.value.w);

            writer.WriteFloat(scale);
        }

        public void Deserialize(ref DataStreamReader reader)
        {
            tick = reader.ReadUInt();
            networkEntityId = reader.ReadULong();

            position = new float3(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
            );
            rotation = new quaternion(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
            );
            scale = reader.ReadFloat();
        }

        #region Implementation

        public PortableFunctionPointer<RpcExecutor.ExecuteDelegate> CompileExecute()
        {
            return INVOKE_EXECUTE_FUNCTION_POINTER;
        }

        [BurstCompile]
        private static void InvokeExecute(ref RpcExecutor.Parameters parameters)
        {
            RpcExecutor.ExecuteCreateRequestComponent<SyncTransformFromServerToClientCommand>(ref parameters);
        }

        private static readonly PortableFunctionPointer<RpcExecutor.ExecuteDelegate> INVOKE_EXECUTE_FUNCTION_POINTER =
            new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);

        #endregion

        #region Sender

        public class ServerCopyTransformSendSystem : RpcCommandRequestSystem<SyncTransformFromServerToClientCommand>
        {
        }

        #endregion
    }
}