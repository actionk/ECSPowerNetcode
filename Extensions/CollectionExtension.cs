using System.Collections.Generic;
using Unity.Collections;

namespace Plugins.ECSPowerNetcode.Extensions
{
    public static class CollectionExtension
    {
        public static FixedListInt512 ToFixedListInt512(this IEnumerable<int> array)
        {
            var list = new FixedListInt512();
            foreach (var entry in array)
                list.AddNoResize(entry);
            return list;
        }

        public static int[] ToArray(this INativeList<int> nativeList)
        {
            var array = new int[nativeList.Length];
            for (var i = 0; i < nativeList.Length; i++)
                array[i] = nativeList[i];
            return array;
        }
    }
}