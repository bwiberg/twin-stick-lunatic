﻿// ----------------------------------------------------------------------------
// LICENSE
// This is free and unencumbered software released into the public domain.
//
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
//
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// For more information, please refer to <http://unlicense.org>
// ----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// Tuple structs for use in .NET Not-Quite-3.5 (e.g. Unity3D).
//
// Used Chapter 3 in http://functional-programming.net/ as a starting point.
//
// Note: .NET 4.0 Tuples are immutable classes so they're *slightly* different.
// ----------------------------------------------------------------------------

using System;

namespace Utility {
    /// <summary>
    /// Represents a functional tuple that can be used to store
    /// two values of different types inside one object.
    /// </summary>
    /// <typeparam name="T1">The type of the first element</typeparam>
    /// <typeparam name="T2">The type of the second element</typeparam>
    /// <typeparam name="T3">The type of the third element</typeparam>
    /// <typeparam name="T4">The type of the fourth element</typeparam>
    public sealed class Tuple<T1, T2, T3, T4> {
        private readonly T1 item1;
        private readonly T2 item2;
        private readonly T3 item3;
        private readonly T4 item4;

        /// <summary>
        /// Retyurns the first element of the tuple
        /// </summary>
        public T1 Item1 {
            get { return item1; }
        }

        /// <summary>
        /// Returns the second element of the tuple
        /// </summary>
        public T2 Item2 {
            get { return item2; }
        }

        /// <summary>
        /// Returns the second element of the tuple
        /// </summary>
        public T3 Item3 {
            get { return item3; }
        }

        /// <summary>
        /// Returns the second element of the tuple
        /// </summary>
        public T4 Item4 {
            get { return item4; }
        }

        /// <summary>
        /// Create a new tuple value
        /// </summary>
        /// <param name="item1">First element of the tuple</param>
        /// <param name="second">Second element of the tuple</param>
        /// <param name="third">Third element of the tuple</param>
        /// <param name="fourth">Fourth element of the tuple</param>
        public Tuple(T1 item1, T2 item2, T3 item3, T4 item4) {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
        }

        public override int GetHashCode() {
            int hash = 17;
            hash = hash * 23 + (item1 == null ? 0 : item1.GetHashCode());
            hash = hash * 23 + (item2 == null ? 0 : item2.GetHashCode());
            hash = hash * 23 + (item3 == null ? 0 : item3.GetHashCode());
            hash = hash * 23 + (item4 == null ? 0 : item4.GetHashCode());
            return hash;
        }

        public override bool Equals(object o) {
            if (o.GetType() != typeof(Tuple<T1, T2, T3, T4>)) {
                return false;
            }

            var other = (Tuple<T1, T2, T3, T4>) o;

            return this == other;
        }

        public static bool operator ==(Tuple<T1, T2, T3, T4> a, Tuple<T1, T2, T3, T4> b) {
            if (object.ReferenceEquals(a, null)) {
                return object.ReferenceEquals(b, null);
            }
            if (a.item1 == null && b.item1 != null) return false;
            if (a.item2 == null && b.item2 != null) return false;
            if (a.item3 == null && b.item3 != null) return false;
            if (a.item4 == null && b.item4 != null) return false;
            return
                a.item1.Equals(b.item1) &&
                a.item2.Equals(b.item2) &&
                a.item3.Equals(b.item3) &&
                a.item4.Equals(b.item4);
        }

        public static bool operator !=(Tuple<T1, T2, T3, T4> a, Tuple<T1, T2, T3, T4> b) {
            return !(a == b);
        }

        public void Unpack(Action<T1, T2, T3, T4> unpackerDelegate) {
            unpackerDelegate(Item1, Item2, Item3, Item4);
        }
    }
}
