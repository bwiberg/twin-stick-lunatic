using UnityEngine;

namespace Utility {
    public static class VectorExtensions {
        ////////////////////////
        // Operator overloads //
        ////////////////////////

        public static Vector2 ElementwiseDivide(this Vector2 lhs, Vector2 rhs) {
            return new Vector2(lhs.x / rhs.x, lhs.y / rhs.y);
        }

        ////////////////////////////
        // SWIZZLE GETTER METHODS //
        ////////////////////////////

        public static Vector2 xy(this Vector3 v) {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 xz(this Vector3 v) {
            return new Vector2(v.x, v.z);
        }

        public static Vector2 yz(this Vector3 v) {
            return new Vector2(v.y, v.z);
        }

        ////////////////////////////
        // SWIZZLE SETTER METHODS //
        ////////////////////////////

        public static Vector3 xy(this Vector3 v, float x, float y) {
            v.x = x;
            v.y = y;
            return v;
        }

        public static Vector3 xy(this Vector3 v, Vector2 xy) {
            return v.xy(xy[0], xy[1]);
        }

        public static Vector3 xz(this Vector3 v, float x, float z) {
            v.x = x;
            v.z = z;
            return v;
        }

        public static Vector3 xz(this Vector3 v, Vector2 xz) {
            return v.xz(xz[0], xz[1]);
        }

        public static Vector3 yz(this Vector3 v, float y, float z) {
            v.y = y;
            v.z = z;
            return v;
        }

        public static Vector3 yz(this Vector3 v, Vector2 yz) {
            return v.yz(yz[0], yz[1]);
        }

        //////////////////////////////////////
        // IMMUTABLE SWIZZLE SETTER METHODS //
        //////////////////////////////////////

        public static Vector3 CopySetX(this Vector3 v, float x) {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 CopySetY(this Vector3 v, float y) {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 CopySetZ(this Vector3 v, float z) {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector3 CopySetXY(this Vector3 v, float x, float y) {
            return new Vector3(x, y, v.z);
        }

        public static Vector3 CopySetXY(this Vector3 v, Vector2 xy) {
            return v.CopySetXY(xy[0], xy[1]);
        }

        public static Vector3 CopySetXZ(this Vector3 v, float x, float z) {
            return new Vector3(x, v.y, z);
        }

        public static Vector3 CopySetXZ(this Vector3 v, Vector2 xz) {
            return v.CopySetXZ(xz[0], xz[1]);
        }

        public static Vector3 CopySetYZ(this Vector3 v, float y, float z) {
            return new Vector3(v.x, y, z);
        }

        public static Vector3 CopySetYZ(this Vector3 v, Vector2 yz) {
            return v.CopySetYZ(yz[0], yz[1]);
        }

        ////////////////////
        // RANDOM METHODS //
        ////////////////////

        public static Vector3 RandomZeroToOne() {
            return new Vector3(Random.value,
                Random.value,
                Random.value);
        }

        public static Vector3 RandomMinusOneToOne() {
            return new Vector3(Random.value * 2 - 1,
                Random.value * 2 - 1,
                Random.value * 2 - 1);
        }
    }
}
