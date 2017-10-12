using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Utility;

namespace ProceduralGeneration {
    public class QuadMesh {
        public static float OptimizationCenterDistanceSqr = 1e-6f;

        public class Quad {
            public readonly Vector3[] vertices = new Vector3[4];

            public Quad() {
            }

            public Quad(Vector3 a0, Vector3 a1, Vector3 b1, Vector3 b0) {
                vertices[0] = a0;
                vertices[1] = a1;
                vertices[2] = b1;
                vertices[3] = b0;
            }

            public Vector3 Center {
                get { return 0.25f * vertices.Aggregate(Vector3.zero, (current, vertex) => current + vertex); }
            }

            public Vector3 Normal {
                get { return Vector3.Cross(vertices[1] - vertices[0], vertices[2] - vertices[0]).normalized; }
            }
        }

        private IList<Vector3> Vertices = new List<Vector3>();
        private IList<Vector2> UVs = new List<Vector2>();
        private IList<IntVector4> Indices = new List<IntVector4>();

        public Mesh ToMesh(bool shareVertices = false, bool convertToTris = true) {
            var mesh = new Mesh();

            Vector3[] vertices;
            Vector2[] uvs;
            if (shareVertices) {
                vertices = Vertices.ToArray();
                uvs = UVs.ToArray();
            }
            else {
                List<Vector3> _vertices = new List<Vector3>();
                List<Vector2> _uvs = new List<Vector2>();

                int indexCounter = 0;
                for (int iquad = 0; iquad < Indices.Count; ++iquad) {
                    for (int ivert = 0; ivert < 4; ++ivert) {
                        _vertices.Add(Vertices[Indices[iquad][ivert]]);
                        _uvs.Add(UVs[Indices[iquad][ivert]]);
                    }
                    Indices[iquad] = new IntVector4(indexCounter, indexCounter + 1, indexCounter + 2, indexCounter + 3);
                    indexCounter += 4;
                }

                vertices = _vertices.ToArray();
                uvs = _uvs.ToArray();
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;

            if (convertToTris) {
                mesh.SetIndices(Indices
                        .SelectMany(quad => new[] {quad[0], quad[1], quad[2], quad[0], quad[2], quad[3]})
                        .ToArray(),
                    MeshTopology.Triangles, 0, true);
            }
            else {
                mesh.SetIndices(Indices
                        .SelectMany(quad => quad.values)
                        .ToArray(),
                    MeshTopology.Quads, 0, true);
            }
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

            return mesh;
        }

        public delegate void VertexModifier(ref Vector3 vertex, ref Vector2 uv);

        public delegate void QuadModifier(Quad quad, int index);

        public void ForEachVertex(VertexModifier modifier) {
            int count = Vertices.Count;
            for (int i = 0; i < count; ++i) {
                Vector3 vertex = Vertices[i];
                Vector2 uv = UVs[i];

                modifier(ref vertex, ref uv);

                Vertices[i] = vertex;
                UVs[i] = uv;
            }
        }

        public void ForEachQuad(QuadModifier modifier) {
            int count = Indices.Count;
            Quad quad = new Quad();
            for (int i = 0; i < count; ++i) {
                IntVector4 indices = Indices[i];
                quad.vertices[0] = Vertices[indices[0]];
                quad.vertices[1] = Vertices[indices[1]];
                quad.vertices[2] = Vertices[indices[2]];
                quad.vertices[3] = Vertices[indices[3]];
                modifier(quad, i);
            }
        }

        public void Extrude(int index, Vector3 extrusionDir, float extrusionLevel) {
            IntVector4 originalFace = Indices[index];

            int vertexCount = Vertices.Count;
            for (int i = 0; i < 4; ++i) {
                Vertices.Add(Vertices[originalFace[i]] + extrusionDir * extrusionLevel);
                UVs.Add(UVs[originalFace[i]]);
            }

            // create upper face
            IntVector4 extrudedFace = new IntVector4(vertexCount, vertexCount + 1, vertexCount + 2, vertexCount + 3);
            Indices[index] = extrudedFace;

            // create side faces
            Indices.Add(new IntVector4(originalFace[0], originalFace[1], extrudedFace[1], extrudedFace[0]));
            Indices.Add(new IntVector4(originalFace[1], originalFace[2], extrudedFace[2], extrudedFace[1]));
            Indices.Add(new IntVector4(originalFace[2], originalFace[3], extrudedFace[3], extrudedFace[2]));
            Indices.Add(new IntVector4(originalFace[3], originalFace[0], extrudedFace[0], extrudedFace[3]));
        }

        public void Optimize() {
            /*
            HashSet<int> indicesToKeep = new HashSet<int>();
            for (int i = 0; i < Indices.Count; ++i) {
                indicesToKeep.Add(i);
            }
            
            for (int i = Indices.Count - 1; i >= 0; --i) {
                if (!indicesToKeep.Contains(i)) {
                    continue;
                }
                Quad iquad = QuadAtIndex(i);
                for (int j = i - 1; j >= 0; --j) {
                    if (!indicesToKeep.Contains(j)) {
                        continue;
                    }
                    Quad jquad = QuadAtIndex(j);
                    if ((iquad.Center - jquad.Center).sqrMagnitude < OptimizationCenterDistanceSqr) {
                        // remove i first since i > j
                        indicesToKeep.Remove(i);
                        indicesToKeep.Remove(j);
                    }
                }
            }
            
            Indices = indicesToKeep.Select(index => Indices[index]).ToList();
            */
        }

        private Quad QuadAtIndex(int index) {
            Quad quad = new Quad();
            for (int vert = 0; vert < 4; ++vert) {
                quad.vertices[vert] = Vertices[Indices[index][vert]];
            }
            return quad;
        }

        public static QuadMesh CreatePlane(Vector2 dimensions, IntVector2 faces) {
            QuadMesh mesh = new QuadMesh();

            IntVector2 numVertices = faces + IntVector2.one;

            Vector2 step = dimensions / faces;
            IntVector2 index = IntVector2.zero;

            // vertices
            for (index.x = 0; index.x < numVertices.x; ++index.x) {
                for (index.y = 0; index.y < numVertices.y; ++index.y) {
                    Vector2 position = index * step;

                    mesh.Vertices.Add(new Vector3(position[0], 0.0f, position[1]));
                    mesh.UVs.Add(position.ElementwiseDivide(dimensions));
                }
            }

            // indices
            for (index.x = 0; index.x < faces.x; ++index.x) {
                for (index.y = 0; index.y < faces.y; ++index.y) {
                    IntVector4 quad = IntVector4.zero;
                    quad[0] = LinearizeIndex(index, numVertices);
                    quad[1] = LinearizeIndex(index + new IntVector2(1, 0), numVertices);
                    quad[2] = LinearizeIndex(index + new IntVector2(1, 1), numVertices);
                    quad[3] = LinearizeIndex(index + new IntVector2(0, 1), numVertices);

                    mesh.Indices.Add(quad);
                }
            }

            return mesh;
        }

        public static int LinearizeIndex(IntVector2 index, IntVector2 dimensions) {
            Assert.IsTrue(index.x >= 0 && index.x < dimensions.x);
            Assert.IsTrue(index.y >= 0 && index.y < dimensions.y);

            return index.x + index.y * dimensions.x;
        }

        public static IntVector2 UnlinearizeIndex(int index, IntVector2 dimensions) {
            IntVector2 indices = IntVector2.zero;
            indices.x = index % dimensions.x;
            indices.y = (index - indices.x) / dimensions.x;
            return indices;
        }
    }
}
