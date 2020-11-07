using System;
using Plugins.ECSPowerNetcode.Worlds;

namespace Plugins.ECSPowerNetcode.Archetypes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NetcodeArchetypeAttribute : Attribute
    {
        public WorldType WorldType { get; set; }
    }
}