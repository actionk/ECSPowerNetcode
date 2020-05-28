namespace Plugins.ECSPowerNetcode.Server.Entities
{
    public class DefaultNetworkEntityIdFactory : INetworkEntityIdFactory
    {
        private ulong m_nextEntityId = 1;

        public ulong NextId()
        {
            return m_nextEntityId++;
        }
    }
}