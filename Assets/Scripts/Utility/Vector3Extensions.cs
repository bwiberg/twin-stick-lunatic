using UnityEngine;

namespace Utility {
    public static class Vector3Extensions {
        public static Vector2 xy(this Vector3 v) {
            return new Vector2(v.x, v.y);
        }
        
        public static Vector2 xz(this Vector3 v) {
            return new Vector2(v.x, v.z);
        }
        
        public static Vector2 yz(this Vector3 v) {
            return new Vector2(v.y, v.z);
        }
    }
}
