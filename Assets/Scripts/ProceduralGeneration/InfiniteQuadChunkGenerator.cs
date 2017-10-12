using System;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
using Utility;

namespace ProceduralGeneration {
    public class InfiniteQuadChunkGenerator : InfiniteTerrainChunkGenerator {
        [SerializeField] private bool RandomizeSeed;
        [SerializeField] private InfiniteTerrainChunk ChunkPrefab;

        [SerializeField] private int SubdivisionsX = 10;
        [SerializeField] private int SubdivisionsY = 10;

        [SerializeField] private Vector3[] OctavesScaleAndHeightAndMinNoise;
        [SerializeField] private bool RoundNoise;

        [SerializeField] private bool FlatShading = true;
        [SerializeField] private bool ConvertToTris = true;

        public override void CreateChunk(Vector2 center, Vector2 dimensions, ChunkCreatedCallback callback) {
            var bw = new BackgroundWorker();

            bw.DoWork += delegate(object o, DoWorkEventArgs args) {
                args.Result = GenerateTerrain(center, dimensions);
            };

            bw.RunWorkerCompleted += delegate(object o, RunWorkerCompletedEventArgs args) {
                var quadMesh = args.Result as QuadMesh;
                
                var mesh = quadMesh.ToMesh(!FlatShading, ConvertToTris);
                var chunk = Instantiate(ChunkPrefab.gameObject).GetComponent<InfiniteTerrainChunk>();
                chunk.gameObject.SetActive(false);
                chunk.SetMesh(mesh);
                chunk.Center = center;
                chunk.Dimensions = dimensions;

                // set correct position
                chunk.transform.position = chunk.transform.position.CopySetXZ(center - dimensions / 2);

                callback(chunk);
            };

            bw.RunWorkerAsync();
        }

        private QuadMesh GenerateTerrain(Vector2 center, Vector2 dimensions) {
            var quadMesh = QuadMesh.CreatePlane(dimensions, new IntVector2(SubdivisionsX, SubdivisionsY));

            quadMesh.ForEachQuad((quad, index) => {
                float height = GenerateHeight(center, quad.vertices[0].x, quad.vertices[0].z);

                if (height == 0.0f) return;
                quadMesh.Extrude(index, quad.Normal, height);
            });

            quadMesh.Optimize();
            return quadMesh;
        }

        private float GenerateHeight(Vector2 center, float x, float z) {
            float sum = 0.0f;

            foreach (var v in OctavesScaleAndHeightAndMinNoise) {
                float scale = v[0];
                float height = v[1];
                float minNoise = v[2];

                float octaveX = (center[0] + x) * scale;
                float octaveZ = (center[1] + z) * scale;
                if (RoundNoise) {
                    octaveX = Mathf.Round(octaveX);
                    octaveZ = Mathf.Round(octaveZ);
                }

                float noise = Mathf.PerlinNoise(
                    PerlinNoiseOffset + octaveX,
                    PerlinNoiseOffset + octaveZ);
                noise = Mathf.Max(noise - minNoise, 0.0f) / (1.0f - minNoise);

                sum += height * noise;
            }

            return sum;
        }

        private float PerlinNoiseOffset;

        private void Awake() {
            PerlinNoiseOffset = 1e4f + (RandomizeSeed ? UnityEngine.Random.value * 1e6f : 0.0f);
        }
    }
}
