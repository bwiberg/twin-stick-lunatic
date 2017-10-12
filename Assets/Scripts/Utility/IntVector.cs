using System;
using UnityEngine;

namespace Utility {
    public struct IntVector2 {
        public int x;
        public int y;

        public int this[int index] {
            get {
                switch (index) {
                    case 0:
                        return x;
                    case 1:
                        return y;
                }
                throw new IndexOutOfRangeException(
                    string.Format("Tried to access IntVector2 element at index {0}, but max index is 1", index));
            }
            set {
                switch (index) {
                    case 0:
                        x = value;
                        return;
                    case 1:
                        y = value;
                        return;
                }
                throw new IndexOutOfRangeException(
                    string.Format("Tried to access IntVector2 element at index {0}, but max index is 1", index));
            }
        }

        public IntVector2(int value) {
            x = y = value;
        }

        public IntVector2(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public IntVector2(IntVector2 other) {
            x = other.x;
            y = other.y;
        }

        public override string ToString() {
            return string.Format("[{0}, {1}]", x, y);
        }

        public override bool Equals(object obj) {
            if (!(obj is IntVector2)) {
                return false;
            }

            var other = (IntVector2) obj;
            return this == other;
        }

        public override int GetHashCode() {
            return 23 + 139 * x + y;
        }

        public int[] values {
            get { return new[] {x, y}; }
        }

        public static IntVector2 operator +(IntVector2 lhs, IntVector2 rhs) {
            return new IntVector2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static IntVector2 operator -(IntVector2 lhs, IntVector2 rhs) {
            return new IntVector2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static IntVector2 operator *(IntVector2 lhs, IntVector2 rhs) {
            return new IntVector2(lhs.x * rhs.x, lhs.y * rhs.y);
        }

        public static IntVector2 operator /(IntVector2 lhs, IntVector2 rhs) {
            return new IntVector2(lhs.x / rhs.x, lhs.y / rhs.y);
        }

        public static bool operator ==(IntVector2 lhs, IntVector2 rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(IntVector2 lhs, IntVector2 rhs) {
            return !(lhs == rhs);
        }

        public static IntVector2 zero {
            get { return new IntVector2(0); }
        }

        public static IntVector2 one {
            get { return new IntVector2(1); }
        }

        //////////////////////////////////////////
        // Extensions methods for float Vector2 //
        //////////////////////////////////////////

        public static Vector2 operator *(IntVector2 lhs, Vector2 rhs) {
            return new Vector2(lhs.x * rhs.x, lhs.y * rhs.y);
        }

        public static Vector2 operator *(Vector2 lhs, IntVector2 rhs) {
            return new Vector2(lhs.x * rhs.x, lhs.y * rhs.y);
        }

        public static Vector2 operator /(IntVector2 lhs, Vector2 rhs) {
            return new Vector2(lhs.x / rhs.x, lhs.y / rhs.y);
        }

        public static Vector2 operator /(Vector2 lhs, IntVector2 rhs) {
            return new Vector2(lhs.x / rhs.x, lhs.y / rhs.y);
        }
    }

    public struct IntVector3 {
        public int x;
        public int y;
        public int z;

        public int this[int index] {
            get {
                switch (index) {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                }
                throw new IndexOutOfRangeException(
                    string.Format("Tried to access IntVector3 element at index {0}, but max index is 2", index));
            }
            set {
                switch (index) {
                    case 0:
                        x = value;
                        return;
                    case 1:
                        y = value;
                        return;
                    case 2:
                        z = value;
                        return;
                }
                throw new IndexOutOfRangeException(
                    string.Format("Tried to access IntVector2 element at index {0}, but max index is 1", index));
            }
        }

        public IntVector3(int value) {
            x = y = z = value;
        }


        public IntVector3(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public IntVector3(IntVector3 other) {
            x = other.x;
            y = other.y;
            z = other.z;
        }

        public override string ToString() {
            return string.Format("[{0}, {1}, {2}]", x, y, z);
        }
        
        
        public override bool Equals(object obj) {
            if (!(obj is IntVector3)) {
                return false;
            }

            var other = (IntVector3) obj;
            return this == other;
        }

        public override int GetHashCode() {
            return 23 + 139 * (139 * x + y) + z;
        }

        public int[] values {
            get { return new[] {x, y, z}; }
        }

        public static IntVector3 operator +(IntVector3 lhs, IntVector3 rhs) {
            return new IntVector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static IntVector3 operator -(IntVector3 lhs, IntVector3 rhs) {
            return new IntVector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static IntVector3 operator *(IntVector3 lhs, IntVector3 rhs) {
            return new IntVector3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }

        public static IntVector3 operator /(IntVector3 lhs, IntVector3 rhs) {
            return new IntVector3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }

        public static bool operator ==(IntVector3 lhs, IntVector3 rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(IntVector3 lhs, IntVector3 rhs) {
            return !(lhs == rhs);
        }

        public static IntVector3 zero {
            get { return new IntVector3(0); }
        }

        public static IntVector3 one {
            get { return new IntVector3(1); }
        }

        //////////////////////////////////////////
        // Extensions methods for float Vector3 //
        //////////////////////////////////////////

        public static Vector3 operator *(IntVector3 lhs, Vector3 rhs) {
            return new Vector3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }

        public static Vector3 operator *(Vector3 lhs, IntVector3 rhs) {
            return new Vector3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }

        public static Vector3 operator /(IntVector3 lhs, Vector3 rhs) {
            return new Vector3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }

        public static Vector3 operator /(Vector3 lhs, IntVector3 rhs) {
            return new Vector3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }
    }

    public struct IntVector4 {
        public int x;
        public int y;
        public int z;
        public int w;

        public int this[int index] {
            get {
                switch (index) {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                }
                throw new IndexOutOfRangeException(
                    string.Format("Tried to access IntVector4 element at index {0}, but max index is 3", index));
            }
            set {
                switch (index) {
                    case 0:
                        x = value;
                        return;
                    case 1:
                        y = value;
                        return;
                    case 2:
                        z = value;
                        return;
                    case 3:
                        w = value;
                        return;
                }
                throw new IndexOutOfRangeException(
                    string.Format("Tried to access IntVector4 element at index {0}, but max index is 3", index));
            }
        }

        public IntVector4(int value) {
            x = y = z = w = value;
        }


        public IntVector4(int x, int y, int z, int w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public IntVector4(IntVector4 other) {
            x = other.x;
            y = other.y;
            z = other.z;
            w = other.w;
        }

        public override string ToString() {
            return string.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
        }
        
        
        public override bool Equals(object obj) {
            if (!(obj is IntVector4)) {
                return false;
            }

            var other = (IntVector4) obj;
            return this == other;
        }

        public override int GetHashCode() {
            return 23 + 139 * (139 * (139 * x + y) + z) + w;
        }

        public int[] values {
            get { return new[] {x, y, z, w}; }
        }

        public static IntVector4 operator +(IntVector4 lhs, IntVector4 rhs) {
            return new IntVector4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
        }

        public static IntVector4 operator -(IntVector4 lhs, IntVector4 rhs) {
            return new IntVector4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
        }

        public static IntVector4 operator *(IntVector4 lhs, IntVector4 rhs) {
            return new IntVector4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
        }

        public static IntVector4 operator /(IntVector4 lhs, IntVector4 rhs) {
            return new IntVector4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
        }

        public static bool operator ==(IntVector4 lhs, IntVector4 rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z && lhs.w == rhs.w;
        }

        public static bool operator !=(IntVector4 lhs, IntVector4 rhs) {
            return !(lhs == rhs);
        }

        public static IntVector4 zero {
            get { return new IntVector4(0); }
        }

        public static IntVector4 one {
            get { return new IntVector4(1); }
        }

        //////////////////////////////////////////
        // Extensions methods for float Vector4 //
        //////////////////////////////////////////

        public static Vector4 operator *(IntVector4 lhs, Vector4 rhs) {
            return new Vector4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
        }

        public static Vector4 operator *(Vector4 lhs, IntVector4 rhs) {
            return new Vector4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
        }

        public static Vector4 operator /(IntVector4 lhs, Vector4 rhs) {
            return new Vector4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
        }

        public static Vector4 operator /(Vector4 lhs, IntVector4 rhs) {
            return new Vector4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
        }
    }
}
