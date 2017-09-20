using UnityEngine;

namespace Utility {
    public static class ArrayExtensions {
        public static T GetRandom<T>(this T[] array) {
            return array[Random.Range(0, array.Length - 1)];
        }
        
        public static T[] Fill<T>(this T[] array, T value) {
            for (int i = 0; i < array.Length; ++i) {
                array[i] = value;
            }
            return array;
        }
    }
}
