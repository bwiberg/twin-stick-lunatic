using System.Linq;
using UnityEngine;

namespace Utility {
    public static class ArrayExtensions {
        public static T GetRandom<T>(this T[] array) {
            return array[Random.Range(0, array.Length - 1)];
        }

        public static T[] GetRandom<T>(this T[] array, int N) {
            return array.OrderBy(t => Random.value).Take(N).ToArray();
        }

        public static T[] Fill<T>(this T[] array, T value) {
            for (int i = 0; i < array.Length; ++i) {
                array[i] = value;
            }
            return array;
        }

        public static string ToStringWithSeparator<T>(this T[] array, string separator = ",") {
            return string.Join(separator, array.Select(t => t.ToString()).ToArray());
        }
    }
}
