﻿using Plugins.Shared.ECSPowerNetcode.Worlds;
using Unity.Entities;
using Unity.NetCode;
using UnityEditor.UIElements;

[assembly: UxmlNamespacePrefix("Plugins.ECSPowerNetcode", "first")]

namespace Plugins.Shared.ECSPowerNetcode
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