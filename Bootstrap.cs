using Plugins.ECSPowerNetcode.Worlds;
using Unity.Entities;
using Unity.NetCode;

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
            ScriptBehaviourUpdateOrder.UpdatePlayerLoop(world);

            CreateClientWorld(world, "ClientWorld");
            CreateServerWorld(world, "ServerWorld");

            EntityWorldManager.Instance.Initialize();
            return true;
        }
    }
}