using System.Net;
using Plugins.Shared.ECSPowerNetcode.Client.Components;
using Plugins.Shared.ECSPowerNetcode.Client.Groups;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

namespace Plugins.Shared.ECSPowerNetcode.Client.Lifecycle
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

            var address = IPAddress.Parse(connectToServer.host.ToString());
            ep.SetRawAddressBytes(new NativeArray<byte>(address.GetAddressBytes(), Allocator.Temp));

            network.Connect(ep);

            Debug.Log($"[Client] Connecting to server on {connectToServer.host.ToString()}:{ep.Port}");
        }
    }
}