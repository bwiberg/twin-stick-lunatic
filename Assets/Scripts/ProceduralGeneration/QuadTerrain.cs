using UnityEngine;
using Utility;

namespace ProceduralGeneration {
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
    public class QuadTerrain : CustomBehaviour {
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        [SerializeField] private bool GenerateEachFrame;
        
        [SerializeField] private Vector2 Dimensions;
        [SerializeField] private int SubdivisionsX = 10;
        [SerializeField] private int SubdivisionsY = 10;

        [SerializeField] private float NoiseDetail = 1.0f;
        [SerializeField] private bool RoundNoise;
        [SerializeField, Range(0, 1)] private float MinNoiseValue = 0.0f;
        [SerializeField] private float MaxTerrainHeight = 1.0f;

        [SerializeField] private bool FlatShading = true;
        [SerializeField] private bool ConvertToTris = true;

        private void Awake() {
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();

            UpdateTerrain();
        }

        private void Update() {
            if (GenerateEachFrame) {
                UpdateTerrain();
            }
        }

        private void UpdateTerrain() {
            Mesh mesh = GenerateTerrain();

            meshFilter.sharedMesh = mesh;
            meshCollider.sharedMesh = mesh;
        }

        private Mesh GenerateTerrain() {
            QuadMesh quadMesh = QuadMesh.CreatePlane(Dimensions, new IntVector2(SubdivisionsX, SubdivisionsY));
            quadMesh.ForEachVertex((ref Vector3 vertex, ref Vector2 uv) => {
                // vertex.y = vertex.x + vertex.z;
            });
            if (MinNoiseValue != 1.0f) {
                quadMesh.ForEachQuad((quad, index) => {
                    float noise = GenerateNoise(quad.vertices[0].x, quad.vertices[0].z);
                    noise = Mathf.Max(noise - MinNoiseValue, 0.0f) / (1.0f - MinNoiseValue);

                    if (noise != 0.0f) {
                        float height = MaxTerrainHeight * noise;
                        quadMesh.Extrude(index, quad.Normal, height);
                    }
                });
            }
            quadMesh.Optimize();
            return quadMesh.ToMesh(!FlatShading, ConvertToTris);
        }

        private float GenerateNoise(float x, float z) {
            x *= NoiseDetail;
            z *= NoiseDetail;
            if (RoundNoise) {
                x = Mathf.Round(x);
                z = Mathf.Round(z);
            }
            return Mathf.PerlinNoise(x, z);
        }
    }
}
