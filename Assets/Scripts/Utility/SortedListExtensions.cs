using System.Collections.Generic;

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
    }
}
