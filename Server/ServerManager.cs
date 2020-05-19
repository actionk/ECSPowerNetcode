using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.ECSEntityBuilder.Worlds;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Server
{
    public class ServerManager : ANetworkEntityManager
    {
        public struct ClientConnection
        {
            public int networkId;
            public Entity connectionEntity;
            public Entity commandHandlerEntity;
        }

        private ulong nextEntityId = 1;
        public ulong NextNetworkEntityId => nextEntityId++;

        private readonly Dictionary<int, ClientConnection> m_openedConnections = new Dictionary<int, ClientConnection>();

        public void OnConnected(int networkId, Entity connectionEntity, Entity commandHandlerEntity)
        {
            m_openedConnections[networkId] = new ClientConnection
            {
                networkId = networkId,
                connectionEntity = connectionEntity,
                commandHandlerEntity = commandHandlerEntity
            };
        }

        public void OnDisconnected(int networkId)
        {
            m_openedConnections.Remove(networkId);
        }

        public ClientConnection GetClientConnectionByNetworkId(int networkId)
        {
            return m_openedConnections[networkId];
        }

        public void StartServer(ushort port)
        {
            var serverWorld = WorldManager.Instance.Server;
            if (!serverWorld.IsCreated)
                throw new NotImplementedException("Server world doesn't exist!");

            var startServerEntity = serverWorld.EntityManager.CreateEntity();
            serverWorld.EntityManager.AddComponentData(startServerEntity, new StartServer {port = port});
        }

        public List<ClientConnection> AllConnections => m_openedConnections.Values.ToList();

        #region Singleton

        private static ServerManager INSTANCE = new ServerManager();

        static ServerManager()
        {
        }

        private ServerManager()
        {
        }

        public static ServerManager Instance
        {
            get { return INSTANCE; }
        }

#if UNITY_EDITOR
        // for quick play mode entering 
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            INSTANCE = new ServerManager();
        }
#endif

        #endregion
    }
}