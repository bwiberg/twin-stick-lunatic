using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Utility;

namespace Genetics {
    public class DNA {
        public static int Count { get; private set; }

        public readonly float[] Genes;
        public readonly int ID;

        public uint Length {
            get { return (uint) Genes.Length; }
        }

        public DNA(uint length) {
            Genes = new float[length];

            for (uint i = 0; i < length; ++i) {
                Genes[i] = Random.Range(-1.0f, 1.0f);
            }

            ID = Count++;
        }

        public DNA(float[] genes) {
            Genes = genes;
            Count++;
        }

        public override string ToString() {
            return string.Format("[genes={0}, ID={1}]", Genes.ToStringWithSeparator(), ID);
        }

        public override bool Equals(object obj) {
            if (!(obj is DNA)) {
                return false;
            }

            var dna = (DNA) obj;

            return Genes.SequenceEqual(dna.Genes);
        }

        public static Tuple<DNA, DNA> Breed(DNA a, DNA b) {
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

        static DNA() {
            Count = 0;
        }
    }
}
