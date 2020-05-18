using Plugins.ECSPowerNetcode.Client.Components;
using Plugins.ECSPowerNetcode.Client.Groups;
using Plugins.UnityExtras.Logs;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;

namespace Plugins.ECSPowerNetcode.Client.Lifecycle
{
    [UpdateInGroup(typeof(ClientConnectionSystemGroup))]
    public class ClientConnectToServerSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<ConnectToServer>();
        }

        protected override void OnUpdate()
        {
            var connectToServer = GetSingleton<ConnectToServer>();
            EntityManager.DestroyEntity(GetSingletonEntity<ConnectToServer>());

            var network = World.GetExistingSystem<NetworkStreamReceiveSystem>();

            NetworkEndPoint ep = NetworkEndPoint.LoopbackIpv4;
            ep.Port = connectToServer.port;

            network.Connect(ep);
            UnityLogger.Log($"[Client] Connected to server on {ep.Port} port");
        }
    }
}