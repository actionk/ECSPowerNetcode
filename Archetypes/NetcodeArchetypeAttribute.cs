using System;
using Plugins.Shared.ECSPowerNetcode.Worlds;

namespace Plugins.Shared.ECSPowerNetcode.Archetypes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NetcodeArchetypeAttribute : Attribute
    {
        public WorldType WorldType { get; set; }
    }
}