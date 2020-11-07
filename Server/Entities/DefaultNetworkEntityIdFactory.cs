namespace Plugins.ECSPowerNetcode.Server.Entities
{
    public class DefaultNetworkEntityIdFactory : INetworkEntityIdFactory
    {
        private uint m_nextEntityId = 1;

        public uint NextId()
        {
            return m_nextEntityId++;
        }
    }
}