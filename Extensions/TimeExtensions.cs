using Unity.Core;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Extensions
{
    public static class TimeExtensions
    {
        public static ulong ElapsedTimeMs(this TimeData timeData)
        {
            return (ulong) (timeData.ElapsedTime * 1000);
        }

        public static ulong FixedTimeMs => (ulong) (Time.fixedTime * 1000);
    }
}