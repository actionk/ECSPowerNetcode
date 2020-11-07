using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Server.Components
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