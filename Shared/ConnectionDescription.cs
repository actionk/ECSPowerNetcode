using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Shared
{
    public class ConnectionDescription
    {
        public readonly int networkConnectionId;
        public readonly Entity connectionEntity;
        public readonly Entity commandHandlerEntity;

        public ConnectionDescription(int networkConnectionId, Entity connectionEntity, Entity commandHandlerEntity)
        {
            this.networkConnectionId = networkConnectionId;
            this.connectionEntity = connectionEntity;
            this.commandHandlerEntity = commandHandlerEntity;
        }

        public bool IsEmpty => networkConnectionId == 0;
    }
}