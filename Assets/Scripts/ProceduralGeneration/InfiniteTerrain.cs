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
        [SerializeField, Range(1, 5)] private int DeletionRadius = 1;

        [SerializeField] private IDictionary<IntVector2, InfiniteTerrainChunk> ChunksByIndex2 =
            new Dictionary<IntVector2, InfiniteTerrainChunk>();

        private Queue<IntVector2> ChunksToGenerate = new Queue<IntVector2>();

        private IntVector2 PlayerIndex2 {
            get {
                var posXZ = player.transform.position.xz();
                var chunkXZ = posXZ.ElementwiseDivide(ChunkDimensions);
                var indicesXZ = new IntVector2(Mathf.RoundToInt(chunkXZ[0]), Mathf.RoundToInt(chunkXZ[1]));
                
                return indicesXZ;
            }    
        }
        
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
                    
                    // Debug.LogFormat("Created '{0}'", chunk.gameObject.name);
                });
                
                chunksGenerated++;
            }

            DeleteChunks();
        }

        private IEnumerable<IntVector2> ComputeNewChunkIndices() {
            var newChunkIndices = new List<IntVector2>();

            var indicesXZ = PlayerIndex2;
            for (int dx = -GenerationRadius;
                dx <= GenerationRadius;
                dx++) {
                for (int dy = -GenerationRadius; dy <= GenerationRadius; dy++) {
                    var indices = indicesXZ + new IntVector2(dx, dy);
                    if (ChunksToGenerate.Contains(indices) || ChunksByIndex2.ContainsKey(indices))
                        continue;

                    newChunkIndices.Add(indices);
                }
            }
            return newChunkIndices;
        }

        private void DeleteChunks() {
            var indicesToDelete = new List<IntVector2>();
            
            var indicesXZ = PlayerIndex2;
            foreach (var kvp in ChunksByIndex2) {
                var deltaIndex = indicesXZ - kvp.Key;
                if (Mathf.Abs(deltaIndex[0]) < DeletionRadius && Mathf.Abs(deltaIndex[1]) < DeletionRadius) 
                    continue;
                
                // Debug.LogFormat("Deleted '{0}'", kvp.Value.gameObject.name);
                indicesToDelete.Add(kvp.Key);
                Destroy(kvp.Value.gameObject);
            }
            
            indicesToDelete.ForEach(t => ChunksByIndex2.Remove(t));
        }
    }
}
