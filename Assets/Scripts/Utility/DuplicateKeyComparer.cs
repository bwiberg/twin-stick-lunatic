using System;
using System.Collections.Generic;

namespace Utility {
    public enum SequenceOrdering {
        Forward,
        Reverse
    }
    
    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable {
        private int sign = 1;

        public DuplicateKeyComparer(SequenceOrdering ordering = SequenceOrdering.Forward) {
            if (ordering == SequenceOrdering.Reverse) {
                sign = -1;
            }
        }

        public int Compare(TKey x, TKey y) {
            int result = sign * x.CompareTo(y);
            return result == 0 ? 1 : result;
        }
    }
}
