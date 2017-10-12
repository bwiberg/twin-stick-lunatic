using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ProceduralGeneration {
    public class InfiniteTerrain : CustomSingletonBehaviour<InfiniteTerrain> {
        [SerializeField] private float SecondsBetweenUpdates = 1.0f;
        [SerializeField] private InfiniteTerrainChunkGenerator ChunkGenerator;
        [SerializeField] private Vector2 ChunkDimensions;
        [SerializeField, Range(1, 5)] private int GenerationRadius = 1;

        [SerializeField] private IDictionary<IntVector2, InfiniteTerrainChunk> ChunksByIndex2 =
            new Dictionary<IntVector2, InfiniteTerrainChunk>();

        private Queue<IntVector2> ChunksToGenerate = new Queue<IntVector2>();

        private void Start() {
            StartCoroutine(PeriodicallyUpdateChunks());
        }

        private IEnumerator PeriodicallyUpdateChunks() {
            for (int i = 0;; i++) {
                UpdateChunks(i == 0);
                yield return new WaitForSeconds(SecondsBetweenUpdates);
            }
        }

        private void UpdateChunks(bool generateAllChunksInQueue) {
            ComputeNewChunkIndices().ForEach(t => ChunksToGenerate.Enqueue(t));


            int chunksGenerated = 0;
            while (ChunksToGenerate.Count > 0 && (generateAllChunksInQueue || chunksGenerated == 0)) {
                var index2 = ChunksToGenerate.Dequeue();
                ChunkGenerator.CreateChunk(index2 * ChunkDimensions, ChunkDimensions, chunk => {
                    ChunksByIndex2.Add(index2, chunk);
                    chunk.gameObject.SetActive(true);
                    chunk.transform.SetParent(transform);
                    chunk.gameObject.name = string.Format("Chunk [{0}, {1}]", index2.x, index2.y);
                });
                
                chunksGenerated++;
            }
        }

        private IEnumerable<IntVector2> ComputeNewChunkIndices() {
            var newChunkIndices = new List<IntVector2>();

            // bounds.center = ChunkDimensions * indexToTest;
            var posXZ = player.transform.position.xz();

            var chunkXZ = posXZ.ElementwiseDivide(ChunkDimensions);

            var indicesXZ = new IntVector2(Mathf.RoundToInt(chunkXZ[0]), Mathf.RoundToInt(chunkXZ[1]));
            for (int dx = -GenerationRadius;
                dx <= GenerationRadius;
                dx++) {
                for (int dy = -GenerationRadius; dy <= GenerationRadius; dy++) {
                    var indices = indicesXZ + new IntVector2(dx, dy);
                    if (ChunksByIndex2.ContainsKey(indices))
                        continue;

                    newChunkIndices.Add(indices);
                }
            }
            return newChunkIndices;
        }
    }
}
