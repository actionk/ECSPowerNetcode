using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Shared.ECSPowerNetcode.Server.Components;
using Plugins.Shared.ECSPowerNetcode.Server.Entities;
using Plugins.Shared.ECSPowerNetcode.Shared;
using Plugins.Shared.ECSPowerNetcode.Worlds;
using Unity.Entities;
using UnityEngine;

namespace Plugins.Shared.ECSPowerNetcode.Server
{
    public class ServerManager
    {
        public delegate void OnPlayerConnected(int networkConnectionId, Entity connectionEntity, Entity commandHandlerEntity);

        public delegate void OnPlayerDisconnected(int networkConnectionId);

        public event OnPlayerConnected OnPlayerConnectedHandler;
        public event OnPlayerDisconnected OnPlayerDisconnectedHandler;

        public uint Tick => EntityWorldManager.Instance.ServerTick;

        public INetworkEntityManager NetworkEntityManager { get; set; } = new DefaultNetworkEntityManager();
        public INetworkEntityIdFactory NetworkEntityIdFactory { get; set; } = new DefaultNetworkEntityIdFactory();
        public uint NextNetworkEntityId => NetworkEntityIdFactory.NextId();

        private readonly Dictionary<Entity, ConnectionDescription> m_openedConnectionsByConnectionEntity = new Dictionary<Entity, ConnectionDescription>();
        private readonly Dictionary<int, ConnectionDescription> m_openedConnectionsById = new Dictionary<int, ConnectionDescription>();

        public void OnConnected(int networkConnectionId, Entity connectionEntity, Entity commandHandlerEntity)
        {
            var connectionDescription = new ConnectionDescription(networkConnectionId, connectionEntity, commandHandlerEntity);
            m_openedConnectionsByConnectionEntity[connectionEntity] = connectionDescription;
            m_openedConnectionsById[networkConnectionId] = connectionDescription;
            OnPlayerConnectedHandler?.Invoke(networkConnectionId, connectionEntity, commandHandlerEntity);
        }

        public void OnDisconnected(int networkId)
        {
            m_openedConnectionsById.Remove(networkId);
            OnPlayerDisconnectedHandler?.Invoke(networkId);
        }

        public ConnectionDescription GetClientConnectionByNetworkId(int networkId)
        {
            return m_openedConnectionsById[networkId];
        }

        public ConnectionDescription GetClientConnectionByConnectionEntity(Entity connectionEntity)
        {
            return m_openedConnectionsByConnectionEntity[connectionEntity];
        }

        public void StartServer(ushort port)
        {
            var serverWorld = EntityWorldManager.Instance.Server;
            if (!serverWorld.IsCreated)
                throw new NotImplementedException("Server world doesn't exist!");

            var startServerEntity = serverWorld.EntityManager.CreateEntity();
            serverWorld.EntityManager.AddComponentData(startServerEntity, new StartServer {port = port});
        }

        public List<ConnectionDescription> AllConnections => m_openedConnectionsById.Values.ToList();

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