using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility {
    public static class SortedListExtensions {
        public static void Pop<TKey, TValue>(this SortedList<TKey, TValue> list) {
            if (list.Count == 0) {
                return;
            }

            list.RemoveAt(list.Count - 1);
        }

        public static void KeepFirstN<TKey, TValue>(this SortedList<TKey, TValue> list, int N) {
            while (list.Count > N) {
                list.RemoveAt(list.Count - 1);
            }
        }
        
        public static void KeepFirstN<TKey, TValue>(this SortedList<TKey, TValue> list, uint N) {
            while (list.Count > N) {
                list.RemoveAt(list.Count - 1);
            }
        }

        public static Tuple<int, TValue> GetRandomDistributedByKey<TValue>(this SortedList<float, TValue> list, TValue defaultValue) {
            float keySum = list.Keys.Sum();
            if (keySum <= 0.0f) {
                var index = Random.Range(0, list.Count - 1);
                return Tuple.Create(index, list.Values[index]); // todo optimize
            }

            float rand = Random.value;

            float normKeySumSoFar = 0.0f;
            int i = 0;
            foreach (var kvp in list) {
                normKeySumSoFar += kvp.Key / keySum;
                if (rand < normKeySumSoFar) {
                    return Tuple.Create(i, kvp.Value);
                }
                i++;
            }

            return Tuple.Create(-1, defaultValue);
        }
        
        public static TValue[] GetRandomDistributedByKey<TValue>(this SortedList<float, TValue> list, int N, TValue defaultValue) {
            var selectedValuesByIndex = new Dictionary<int, TValue>(N);

            int i;
            for (i = 0; i < N && i < list.Count; ++i) {
                Tuple<int, TValue> selected;
                do {
                    selected = GetRandomDistributedByKey(list, defaultValue);
                } while (selectedValuesByIndex.ContainsKey(selected.Item1));
                selectedValuesByIndex.Add(selected.Item1, selected.Item2);
            }

            for (; i < N; ++i) {
                selectedValuesByIndex.Add(-i, defaultValue);
            }

            return selectedValuesByIndex.Values.ToArray();
        }
    }
}
