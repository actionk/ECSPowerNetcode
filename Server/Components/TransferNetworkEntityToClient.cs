using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Components
{
    public struct TransferNetworkEntityToClient : IComponentData
    {
        public Entity clientConnection;
    }
}