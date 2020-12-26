using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Components
{
    public struct TransferNetworkEntityToClient : IBufferElementData
    {
        public long clientConnectionId;
    }
}