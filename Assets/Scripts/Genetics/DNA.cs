using System;

namespace Genetics {
    public class DNA {
        public readonly float[] Genes;

        public uint Length {
            get { return (uint) Genes.Length; }
        }

        protected DNA(uint length, Random rng = null) {
            Genes = new float[Length];

            if (rng == null) {
                rng = new Random();
            }

            for (uint i = 0; i < length; ++i) {
                Genes[i] = (float) rng.NextDouble();
            }
        }
    }
}
