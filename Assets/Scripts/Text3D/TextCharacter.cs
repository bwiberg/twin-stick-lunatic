using UnityEngine;

namespace Text3D {
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
    public class TextCharacter : CustomBehaviour {
        [SerializeField] private string character;
        public string Character {
            get { return character; }
        }
    }
}
