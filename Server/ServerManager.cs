using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.ECSEntityBuilder.Worlds;
using Plugins.ECSPowerNetcode.Features.NetworkEntities;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Server.Entities;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Server
{
    public class ServerManager : ANetworkEntityManager
    {
        public delegate void OnPlayerConnected(int networkId, Entity connectionEntity, Entity commandHandlerEntity);

        public delegate void OnPlayerDisconnected(int networkId);

        public event OnPlayerConnected OnPlayerConnectedHandler;
        public event OnPlayerDisconnected OnPlayerDisconnectedHandler;

        public INetworkEntityIdFactory NetworkEntityIdFactory { get; set; } = new DefaultNetworkEntityIdFactory();
        public ulong NextNetworkEntityId => NetworkEntityIdFactory.NextId();

        private readonly Dictionary<int, ConnectionDescription> m_openedConnections = new Dictionary<int, ConnectionDescription>();

        public void OnConnected(int networkId, Entity connectionEntity, Entity commandHandlerEntity)
        {
            m_openedConnections[networkId] = new ConnectionDescription
            {
                networkId = networkId,
                connectionEntity = connectionEntity,
                commandHandlerEntity = commandHandlerEntity
            };
            OnPlayerConnectedHandler?.Invoke(networkId, connectionEntity, commandHandlerEntity);
        }

        public void OnDisconnected(int networkId)
        {
            m_openedConnections.Remove(networkId);
            OnPlayerDisconnectedHandler?.Invoke(networkId);
        }

        public ConnectionDescription GetClientConnectionByNetworkId(int networkId)
        {
            return m_openedConnections[networkId];
        }

        public void StartServer(ushort port)
        {
            var serverWorld = EntityWorldManager.Instance.Server;
            if (!serverWorld.IsCreated)
                throw new NotImplementedException("Server world doesn't exist!");

            var startServerEntity = serverWorld.EntityManager.CreateEntity();
            serverWorld.EntityManager.AddComponentData(startServerEntity, new StartServer {port = port});
        }

        public List<ConnectionDescription> AllConnections => m_openedConnections.Values.ToList();

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