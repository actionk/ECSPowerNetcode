using System;
using System.Collections.Generic;
using Plugins.ECSEntityBuilder.Worlds;
using Plugins.ECSPowerNetcode.Server.Components;
using Plugins.ECSPowerNetcode.Shared;
using Unity.Entities;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Server
{
    public class ServerManager : ANetworkEntityManager
    {
        private ulong nextEntityId = 1;
        public ulong NextNetworkEntityId => nextEntityId++;

        private readonly List<Entity> m_openedConnections = new List<Entity>();

        public void OnConnected(Entity connection)
        {
            m_openedConnections.Add(connection);
        }
        
        public void OnDisconnected(Entity entity)
        {
            m_openedConnections.Remove(entity);
        }

        public void StartServer(ushort port)
        {
            var serverWorld = WorldManager.Instance.Server;
            if (!serverWorld.IsCreated)
                throw new NotImplementedException("Server world doesn't exist!");

            var startServerEntity = serverWorld.EntityManager.CreateEntity();
            serverWorld.EntityManager.AddComponentData(startServerEntity, new StartServer {port = port});
        }

        public List<Entity> AllConnections => m_openedConnections;

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