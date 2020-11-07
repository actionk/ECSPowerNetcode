using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Server.Components
{
    public struct StartServer : IComponentData
    {
        public ushort port;
    }
}