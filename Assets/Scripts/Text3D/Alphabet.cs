using System.Collections.Generic;
using UnityEngine;

namespace Text3D {
    public class Alphabet : CustomSingletonBehaviour<Alphabet> {
        private static readonly string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        [SerializeField] private TextCharacter[] CharacterPrefabs;
        
        private readonly IDictionary<string, TextCharacter> PrefabsByCharacter = new Dictionary<string, TextCharacter>();

        private void Awake() {
            PrefabsByCharacter.Clear();
            foreach (var prefab in CharacterPrefabs) {
                PrefabsByCharacter.Add(prefab.Character, prefab);
            }
        }

        public TextCharacter GetPrefabForCharacter(string character) {
            return PrefabsByCharacter[character];
        }
    }
}
