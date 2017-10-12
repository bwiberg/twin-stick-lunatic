using UnityEngine;

namespace ProceduralGeneration {
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer))]
    public class InfiniteTerrainChunk : CustomBehaviour {
        private MeshFilter MeshFilter {
            get { return GetComponent<MeshFilter>(); }
        }

        private MeshCollider MeshCollider {
            get { return GetComponent<MeshCollider>(); }
        }

        public Vector2 Center;

        public Vector2 Dimensions;

        public Vector2 HalfDimensions {
            get { return Dimensions / 2; }
            set { Dimensions = 2 * value; }
        }

        public Vector2 MinXZ {
            get { return Center - HalfDimensions; }
        }
        
        public Vector2 MaxXZ {
            get { return Center + HalfDimensions; }
        }

        public void SetMesh(Mesh mesh) {
            MeshFilter.sharedMesh = mesh;
            MeshCollider.sharedMesh = mesh;
        }
    }
}
