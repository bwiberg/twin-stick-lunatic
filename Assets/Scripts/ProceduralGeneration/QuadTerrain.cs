using UnityEngine;
using Utility;

namespace ProceduralGeneration {
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
    public class QuadTerrain : CustomBehaviour {
        public static float MaxOffset = 1e4f;

        private MeshFilter MeshFilter {
            get { return GetComponent<MeshFilter>(); }
        }

        private MeshCollider MeshCollider {
            get { return GetComponent<MeshCollider>(); }
        }

        public enum GenerationTime {
            OnAwake,
            EachFrame,
            EditorOnly
        }

        public GenerationTime TimeOfGeneration;
        public Vector2 NormalizedOffset;

        [SerializeField] private Vector2 Dimensions;
        [SerializeField] private int SubdivisionsX = 10;
        [SerializeField] private int SubdivisionsY = 10;

        [SerializeField] private Vector3[] OctavesScaleAndHeightAndMinNoise;
        [SerializeField] private bool RoundNoise;

        [SerializeField] private bool FlatShading = true;
        [SerializeField] private bool ConvertToTris = true;

        private void Awake() {
            if (TimeOfGeneration == GenerationTime.OnAwake) {
                UpdateTerrain();
            }
        }

        private void Update() {
            if (TimeOfGeneration == GenerationTime.EachFrame) {
                UpdateTerrain();
            }
        }

        public void UpdateTerrain() {
            var mesh = GenerateTerrain();

            MeshFilter.sharedMesh = mesh;
            MeshCollider.sharedMesh = mesh;

            transform.position = transform.position.CopySetXZ(-Dimensions / 2);
        }

        private Mesh GenerateTerrain() {
            var quadMesh = QuadMesh.CreatePlane(Dimensions, new IntVector2(SubdivisionsX, SubdivisionsY));

            quadMesh.ForEachQuad((quad, index) => {
                float height = GenerateHeight(quad.vertices[0].x, quad.vertices[0].z);

                if (height == 0.0f) return;
                quadMesh.Extrude(index, quad.Normal, height);
            });

            quadMesh.Optimize();
            return quadMesh.ToMesh(!FlatShading, ConvertToTris);
        }

        private float GenerateHeight(float x, float z) {
            float sum = 0.0f;

            foreach (var v in OctavesScaleAndHeightAndMinNoise) {
                float scale = v[0];
                float height = v[1];
                float minNoise = v[2];

                float octaveX = x * scale;
                float octaveZ = z * scale;
                if (RoundNoise) {
                    octaveX = Mathf.Round(octaveX);
                    octaveZ = Mathf.Round(octaveZ);
                }

                float noise = Mathf.PerlinNoise(
                    octaveX + MaxOffset * NormalizedOffset.x,
                    octaveZ + MaxOffset * NormalizedOffset.y);
                noise = Mathf.Max(noise - minNoise, 0.0f) / (1.0f - minNoise);

                sum += height * noise;
            }

            return sum;
        }
    }
}
