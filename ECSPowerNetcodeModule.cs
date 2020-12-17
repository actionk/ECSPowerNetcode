using Plugins.ECSPowerNetcode.Archetypes;
using Plugins.ECSPowerNetcode.Client;
using Plugins.ECSPowerNetcode.Server;
using Plugins.ECSPowerNetcode.Worlds;
using UnityEngine;

namespace Plugins.ECSPowerNetcode
{
    public static class ECSPowerNetcodeModule
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Cleanup()
        {
            NetcodeEntityArchetypeManager.Reset();
            EntityWorldManager.Reset();
            ClientManager.Reset();
            ServerManager.Reset();
        }
    }
}