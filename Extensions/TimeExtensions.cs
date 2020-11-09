using Unity.Core;
using UnityEngine;

namespace Plugins.ECSPowerNetcode.Extensions
{
    public static class TimeExtensions
    {
        public static ulong ElapsedTimeInMillis(this TimeData timeData)
        {
            return (ulong) (timeData.ElapsedTime * 1000);
        }

        public static uint ElapsedTimeInSeconds(this TimeData timeData)
        {
            return (uint) timeData.ElapsedTime;
        }

        public static ulong FixedTimeInMillis => (ulong) (Time.fixedTime * 1000);
    }
}