using System;
using Plugins.ECSEntityBuilder.Worlds;
using Plugins.ECSPowerNetcode.Client.Components;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Client
{
    public class ClientManager
    {
        public Entity ConnectionEntity { get; private set; }
        public Entity CommandHandlerEntity { get; private set; }
        public int ConnectionNetworkId { get; private set; } = -1;
        public bool IsConnected { get; private set; }

        public void OnConnectionEstablished(Entity connectionEntity, Entity commandHandlerEntity, int networkId)
        {
            ConnectionEntity = connectionEntity;
            CommandHandlerEntity = commandHandlerEntity;
            ConnectionNetworkId = networkId;
            IsConnected = true;
        }

        public void OnDisconnected()
        {
            ConnectionEntity = Entity.Null;
            CommandHandlerEntity = Entity.Null;
            ConnectionNetworkId = -1;
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
            if (ConnectionEntity == Entity.Null)
                return;

            WorldManager.Instance.Client.EntityManager.AddComponent<NetworkStreamRequestDisconnect>(ConnectionEntity);
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