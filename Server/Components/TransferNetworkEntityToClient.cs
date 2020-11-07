using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Components
{
    public struct TransferNetworkEntityToClient : IBufferElementData
    {
        public Entity clientConnection;

        public TransferNetworkEntityToClient(Entity clientConnection)
        {
            this.clientConnection = clientConnection;
        }
    }
}