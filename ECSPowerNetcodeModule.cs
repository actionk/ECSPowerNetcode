using Plugins.ECSPowerNetcode.Archetypes;
using Plugins.ECSPowerNetcode.Worlds;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Plugins.ECSPowerNetcode
{
    public static class ECSPowerNetcodeModule
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Clean()
        {
            // clean module
            NetcodeEntityArchetypeManager.Reset();
            EntityWorldManager.Reset();
            //ClientManager.Reset();
            //ServerManager.Reset();
        }

        public static void Shutdown()
        {
            // shutdown ECS
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            foreach (var w in World.All)
                ScriptBehaviourUpdateOrder.RemoveWorldFromPlayerLoop(w, ref playerLoop);

            PlayerLoop.SetPlayerLoop(playerLoop);

            World.DisposeAllWorlds();

            WordStorage.Instance.Dispose();
            WordStorage.Instance = null;

            // clean module
            Clean();
        }
    }
}