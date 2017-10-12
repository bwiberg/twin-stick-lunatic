using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Utility;
using Random = UnityEngine.Random;
using Tuple = Utility.Tuple;

namespace Genetics {
    [Serializable]
    public class DNA {
        public readonly float[] Genes;
        public readonly GUID guid = GUID.Generate();

        public uint Length {
            get { return (uint) Genes.Length; }
        }

        public DNA(uint length) {
            Genes = new float[length];

            for (uint i = 0; i < length; ++i) {
                Genes[i] = Random.Range(-1.0f, 1.0f);
            }
        }

        public DNA(float[] genes) {
            Genes = genes;
        }

        public override string ToString() {
            return string.Format("[genes={0}, guid={1}]", Genes.ToStringWithSeparator(), guid);
        }

        public override bool Equals(object obj) {
            if (!(obj is DNA)) {
                return false;
            }

            var dna = (DNA) obj;

            return guid.Equals(dna.guid) && Genes.SequenceEqual(dna.Genes);
        }

        public static Utility.Tuple<DNA, DNA> Breed(DNA a, DNA b) {
            Assert.AreEqual(a.Length, b.Length);
            uint length = a.Length;
            
            float[] genes1 = new float[length];
            float[] genes2 = new float[length];

            int crossoverIndex = Mathf.FloorToInt(Random.value * (length - 1));
            for (int i = 0; i < crossoverIndex; ++i) {
                genes1[i] = a.Genes[i];
                genes2[i] = b.Genes[i];
            }
            for (int i = crossoverIndex; i < length; ++i) {
                genes1[i] = b.Genes[i];
                genes2[i] = a.Genes[i];
            }
            
            return Tuple.Create(new DNA(genes1), new DNA(genes2));
        }
    }
}
