using System;
using System.Collections.Generic;
using Plugins.ECSEntityBuilder.Worlds;
using Plugins.ECSPowerNetcode.Client.Components;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Shared;
using Plugins.ECSPowerNetcode.Utils;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Client
{
    public class ClientManager : ANetworkEntityManager
    {
        private readonly MultiValueDictionary<ulong, Entity> m_entitiesWaitingForManagedRpcCommandResult = new MultiValueDictionary<ulong, Entity>();
        private ulong m_currentManagedPacketId = 1;

        public ConnectionDescription ConnectionToServer { get; private set; }
        public bool IsConnected { get; private set; }
        public ulong NextManagedPacketId => m_currentManagedPacketId++;

        public void OnConnectionEstablished(Entity connectionEntity, Entity commandHandlerEntity, int networkId)
        {
            var connectionToServer = ConnectionToServer;
            connectionToServer.connectionEntity = connectionEntity;
            connectionToServer.commandHandlerEntity = commandHandlerEntity;
            connectionToServer.networkId = networkId;
            ConnectionToServer = connectionToServer;
            IsConnected = true;
        }

        public void OnDisconnected()
        {
            IsConnected = false;
        }

        public void ConnectToServer(ushort port)
        {
            var clientWorld = WorldManager.Instance.Client;
            if (!clientWorld.IsCreated)
                throw new NotImplementedException("Client world doesn't exist!");

            Disconnect();

            var connectToServer = clientWorld.EntityManager.CreateEntity();
            clientWorld.EntityManager.AddComponentData(connectToServer, new ConnectToServer {port = port});
        }

        public void Disconnect()
        {
            if (!IsConnected)
                return;

            WorldManager.Instance.Client.EntityManager.AddComponent<NetworkStreamRequestDisconnect>(ConnectionToServer.connectionEntity);
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