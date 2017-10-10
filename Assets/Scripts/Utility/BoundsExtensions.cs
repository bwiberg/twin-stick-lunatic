using UnityEngine;

namespace Utility {
    public static class BoundsExtensions {
        public static Vector3 RandomWithin(this Bounds bounds) {
            return bounds.center + Vector3.Scale(VectorExtensions.RandomMinusOneToOne(), bounds.size / 2);
        }
    }
}
