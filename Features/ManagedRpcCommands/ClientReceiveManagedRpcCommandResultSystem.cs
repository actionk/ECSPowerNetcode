using Plugins.ECSPowerNetcode.Client;
using Plugins.ECSPowerNetcode.Client.Packets;
using Plugins.UnityExtras.Logs;
using Unity.NetCode;

namespace Plugins.ECSPowerNetcode.Features.ManagedRpcCommands
{
    public class ClientReceiveManagedRpcCommandResultSystem : AClientReceiveRpcCommandSystem<ManagedRpcCommandResult>
    {
        protected override void OnCommand(ref ManagedRpcCommandResult command, ref ReceiveRpcCommandRequestComponent requestComponent)
        {
            var entitiesToBeNotified = ClientManager.Instance.GetEntitiesWaitingForManagedPacket(command.packetId);
            if (entitiesToBeNotified == null)
            {
                UnityLogger.Warning($"[Client] No entities waiting for response registered for packet {command.packetId}");
                return;
            }

            foreach (var entity in entitiesToBeNotified)
            {
                PostUpdateCommands.AddComponent(entity, new ManagedRpcCommandResponse
                {
                    packetId = command.packetId,
                    result = command.result
                });
            }
        }
    }
}