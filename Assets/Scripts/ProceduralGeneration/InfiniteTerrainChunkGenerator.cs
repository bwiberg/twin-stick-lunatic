using UnityEngine;

namespace ProceduralGeneration {
    public abstract class InfiniteTerrainChunkGenerator : CustomSingletonBehaviour<InfiniteTerrainChunkGenerator> {
        public delegate void ChunkCreatedCallback(InfiniteTerrainChunk chunk);

        public abstract void CreateChunk(Vector2 center, Vector2 dimensions, ChunkCreatedCallback callback);
    }
}
