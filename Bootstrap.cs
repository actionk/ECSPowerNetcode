using Plugins.ECSPowerNetcode.Worlds;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine.LowLevel;

namespace Plugins.ECSPowerNetcode
{
    public class Bootstrap : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName)
        {
            Initialize(true, true);
            return true;
        }

        public void Initialize(bool createClientWorld, bool createServerWorld)
        {
            var systems = DefaultWorldInitialization.GetAllSystems(WorldSystemFilterFlags.Default);
            GenerateSystemLists(systems);

            var world = new World("DefaultWorld");
            World.DefaultGameObjectInjectionWorld = world;

            DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(world, ExplicitDefaultWorldSystems);

            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            ScriptBehaviourUpdateOrder.AddWorldToPlayerLoop(world, ref currentPlayerLoop);
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);

            if (createClientWorld)
                CreateClientWorld(world, "ClientWorld");

            if (createServerWorld)
                CreateServerWorld(world, "ServerWorld");

            EntityWorldManager.Instance.Initialize();
        }
    }
}