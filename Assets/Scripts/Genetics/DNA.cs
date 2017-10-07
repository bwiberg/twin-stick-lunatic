using UnityEngine;
using UnityEngine.Assertions;

namespace Genetics {
    public class DNA {
        public static int Count { get; private set; };

        public readonly float[] Genes;

        public uint Length {
            get { return (uint) Genes.Length; }
        }

        public DNA(uint length) {
            Genes = new float[Length];

            for (uint i = 0; i < length; ++i) {
                Genes[i] = Random.Range(-1.0f, 1.0f);
            }

            Count++;
        }

        public DNA(float[] genes) {
            Genes = genes;
            Count++;
        }

        public static DNA Breed(DNA a, DNA b) {
            Assert.AreEqual(a.Length, b.Length);
            uint length = a.Length;
            
            float[] genes = new float[length];

            int crossoverIndex = Mathf.FloorToInt(Random.value * (length - 1));
            for (int i = 0; i < crossoverIndex; ++i) {
                genes[i] = a.Genes[i];
            }
            for (int i = crossoverIndex; i < length; ++i) {
                genes[i] = b.Genes[i];
            }
            
            return new DNA(genes);
        }

        static DNA() {
            Count = 0;
        }
    }
}
