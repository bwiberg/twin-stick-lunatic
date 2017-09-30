using UnityEngine;
using Utility;

namespace ProceduralGeneration {
    [RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
    public class QuadTerrain : CustomBehaviour {
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        [SerializeField] private Vector2 Dimensions;
        [SerializeField] private int SubdivisionsX = 10;
        [SerializeField] private int SubdivisionsY = 10;

        [SerializeField] private float NoiseDetail = 1.0f;
        [SerializeField, Range(0, 1)] private float MinNoiseValue = 0.0f;
        [SerializeField] private float MaxTerrainHeight = 1.0f;

        [SerializeField] private bool FlatShading = true;
        [SerializeField] private bool ConvertToTris = true;

        private void Awake() {
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();

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
                    Vector3 center = quad.Center;
                    
                    float noise = Mathf.PerlinNoise(NoiseDetail * Mathf.Round(center.x), NoiseDetail * Mathf.Round(center.z));
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
    }
}
