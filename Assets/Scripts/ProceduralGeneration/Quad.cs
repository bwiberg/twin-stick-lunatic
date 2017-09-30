using UnityEngine;

namespace ProceduralGeneration {
    public struct Quad {
        public readonly Vector3 A0;
        public readonly Vector3 A1;
        public readonly Vector3 B0;
        public readonly Vector3 B1;

        public Quad(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1) {
            A0 = a0;
            A1 = a1;
            B0 = b0;
            B1 = b1;
        }

        public Vector3 Center {
            get { return 0.5f * (A0 + B1); }
        }

        public Vector3 Normal {
            get { return Vector3.Cross(A1 - A0, B0 - A0).normalized; }
        }
    }
}
