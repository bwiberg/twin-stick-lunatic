using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Text3D {
    public class Alphabet : CustomSingletonBehaviour<Alphabet> {
        private static readonly char[] ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        [SerializeField] private TextCharacter[] CharacterPrefabs;

        private readonly IDictionary<char, TextCharacter> PrefabsByCharacter = new Dictionary<char, TextCharacter>();

        private void Awake() {
            PrefabsByCharacter.Clear();
            foreach (var prefab in CharacterPrefabs) {
                PrefabsByCharacter.Add(prefab.Character, prefab);
            }
            Assert.IsTrue(ALPHABET.All(character => PrefabsByCharacter.ContainsKey(character)));
        }

        public TextCharacter GetPrefabForCharacter(char character) {
            return PrefabsByCharacter[character];
        }

        public bool HasCharacter(char character) {
            return PrefabsByCharacter.ContainsKey(character);
        }
    }
}
