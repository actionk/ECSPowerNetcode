using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Server.Components
{
    public struct ServerToClientCommandHandler : IComponentData
    {
        public Entity connectionEntity;
    }
}