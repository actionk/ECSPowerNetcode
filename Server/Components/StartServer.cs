using Unity.Entities;

namespace Plugins.Shared.ECSPowerNetcode.Server.Components
{
    public struct StartServer : IComponentData
    {
        public ushort port;
    }
}