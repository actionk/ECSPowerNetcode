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
            var systems = DefaultWorldInitialization.GetAllSystems(WorldSystemFilterFlags.Default);
            GenerateSystemLists(systems);

            var world = new World(defaultWorldName);
            World.DefaultGameObjectInjectionWorld = world;

            DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(world, ExplicitDefaultWorldSystems);

            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            ScriptBehaviourUpdateOrder.AddWorldToPlayerLoop(world, ref currentPlayerLoop);
            PlayerLoop.SetPlayerLoop(currentPlayerLoop);

            CreateClientWorld(world, "ClientWorld");
            CreateServerWorld(world, "ServerWorld");

            EntityWorldManager.Instance.Initialize();
            return true;
        }
    }
}