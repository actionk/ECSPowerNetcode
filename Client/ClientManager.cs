using System;
using System.Collections.Generic;
using Plugins.ECSEntityBuilder.Worlds;
using Plugins.ECSPowerNetcode.Client.Components;
using Plugins.ECSPowerNetcode.Shared;
using Plugins.ECSPowerNetcode.Utils;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Client
{
    public class ClientManager
    {
        private readonly MultiValueDictionary<ulong, Entity> m_entitiesWaitingForManagedRpcCommandResult = new MultiValueDictionary<ulong, Entity>();
        private ulong m_currentManagedPacketId = 1;

        public delegate void OnConnected(ConnectionDescription connectionDescription);

        public delegate void OnDisconnected();

        public event OnConnected OnConnectedHandler;
        public event OnDisconnected OnDisconnectedHandler;

        public INetworkEntityManager NetworkEntityManager { get; set; } = new DefaultNetworkEntityManager();
        public ConnectionDescription ConnectionToServer { get; private set; }
        public bool IsConnected { get; private set; }
        public ulong NextManagedPacketId => m_currentManagedPacketId++;

        public uint ServerTick => EntityWorldManager.Instance.ClientTick;

        public void OnConnectedToServer(Entity connectionEntity, Entity commandHandlerEntity, int networkConnectionId)
        {
            var connectionToServer = ConnectionToServer;
            connectionToServer.connectionEntity = connectionEntity;
            connectionToServer.commandHandlerEntity = commandHandlerEntity;
            connectionToServer.networkId = networkConnectionId;
            ConnectionToServer = connectionToServer;
            IsConnected = true;

            OnConnectedHandler?.Invoke(connectionToServer);
        }

        public void OnDisconnectedFromServer()
        {
            IsConnected = false;
            OnDisconnectedHandler?.Invoke();
        }

        public void ConnectToServer(ushort port, string host = "127.0.0.1")
        {
            var clientWorld = EntityWorldManager.Instance.Client;
            if (!clientWorld.IsCreated)
                throw new NotImplementedException("Client world doesn't exist!");

            Disconnect();

            var connectToServer = clientWorld.EntityManager.CreateEntity();
            clientWorld.EntityManager.AddComponentData(connectToServer, new ConnectToServer {port = port, host = host});
            clientWorld.EntityManager.CreateEntity(typeof(FixedClientTickRate));
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;

            EntityWorldManager.Instance.Client.EntityManager.AddComponent<NetworkStreamRequestDisconnect>(ConnectionToServer.connectionEntity);
        }

        public void AddEntityWaitingForManagedRpcCommandResult(Entity entity, ulong packetId)
        {
            m_entitiesWaitingForManagedRpcCommandResult.Add(packetId, entity);
        }

        public HashSet<Entity> GetEntitiesWaitingForManagedPacket(ulong commandPacketId)
        {
            if (m_entitiesWaitingForManagedRpcCommandResult.TryGetValue(commandPacketId, out var result))
            {
                m_entitiesWaitingForManagedRpcCommandResult.Remove(commandPacketId);
                return result;
            }

            return null;
        }

        #region Singleton

        private static ClientManager INSTANCE = new ClientManager();

        static ClientManager()
        {
        }

        private ClientManager()
        {
        }

        public static ClientManager Instance
        {
            get { return INSTANCE; }
        }

#if UNITY_EDITOR
        // for quick play mode entering 
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            INSTANCE = new ClientManager();
        }
#endif

        #endregion
    }
}