using Plugins.ECSPowerNetcode.Shared.Systems;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Plugins.ECSPowerNetcode.Features.Synchronization.Transform
{
    [BurstCompile]
    public struct SyncTransformFromServerToClientCommand : IComponentData, IRpcCommandSerializer<SyncTransformFromServerToClientCommand>
    {
        public uint tick;
        public uint networkEntityId;
        public float3 position;
        public quaternion rotation;
        public float scale;

#region Serialization

        public void Serialize(ref DataStreamWriter writer, in RpcSerializerState state, in SyncTransformFromServerToClientCommand data)
        {
            writer.WriteUInt(data.tick);
            writer.WriteUInt(data.networkEntityId);

            writer.WriteFloat(data.position.x);
            writer.WriteFloat(data.position.y);
            writer.WriteFloat(data.position.z);

            writer.WriteFloat(data.rotation.value.x);
            writer.WriteFloat(data.rotation.value.y);
            writer.WriteFloat(data.rotation.value.z);
            writer.WriteFloat(data.rotation.value.w);

            writer.WriteFloat(data.scale);
        }

        public void Deserialize(ref DataStreamReader reader, in RpcDeserializerState state, ref SyncTransformFromServerToClientCommand data)
        {
            data.tick = reader.ReadUInt();
            data.networkEntityId = reader.ReadUInt();

            data.position = new float3(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
            );
            data.rotation = new quaternion(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat()
            );
            data.scale = reader.ReadFloat();
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
            RpcExecutor.ExecuteCreateRequestComponent<SyncTransformFromServerToClientCommand, SyncTransformFromServerToClientCommand>(ref parameters);
        }

        private static readonly PortableFunctionPointer<RpcExecutor.ExecuteDelegate> INVOKE_EXECUTE_FUNCTION_POINTER =
            new PortableFunctionPointer<RpcExecutor.ExecuteDelegate>(InvokeExecute);

#endregion

#region Sender

        public class ServerCopyTransformSendSystem : RpcCommandSendSystem<SyncTransformFromServerToClientCommand, SyncTransformFromServerToClientCommand>
        {
        }

#endregion
    }
}