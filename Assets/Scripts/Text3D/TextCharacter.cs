using UnityEngine;

namespace Text3D {
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
    public class TextCharacter : CustomBehaviour {
        [SerializeField] private char character;

        private MeshFilter meshFilter;

        public MeshFilter MeshFilter {
            get { return meshFilter ?? (meshFilter = GetComponent<MeshFilter>()); }
        }

        private MeshRenderer meshRenderer;

        public MeshRenderer MeshRenderer {
            get { return meshRenderer ?? (meshRenderer = GetComponent<MeshRenderer>()); }
        }

        private BoxCollider boxCollider;

        public BoxCollider BoxCollider {
            get { return boxCollider ?? (boxCollider = GetComponent<BoxCollider>()); }
        }

        public char Character {
            get { return character; }
        }
    }
}
