using Unity.Collections;
using Unity.Entities;

namespace Plugins.ECSPowerNetcode.Client.Components
{
    public struct ConnectToServer : IComponentData
    {
        public ushort port;
        public FixedString32 host;
    }
}