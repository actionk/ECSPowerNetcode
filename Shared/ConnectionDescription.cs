using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Shared
{
    public struct ConnectionDescription
    {
        public int networkId;
        public Entity connectionEntity;
        public Entity commandHandlerEntity;
    }
}