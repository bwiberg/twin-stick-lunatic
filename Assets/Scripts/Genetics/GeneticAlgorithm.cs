using System;
using System.Collections;
using System.Collections.Generic;

namespace Genetics {
    public class Generik<T> {
        public T[] value;
    }
    
    

    public class Individual {
        public DNA dna;
        private Generik<float> k;
        

        public float EvaluateFitness() {
            return 0.0f;
        }
    }

    public class GeneticAlgorithm {
        public GeneticAlgorithm() {
        }
    }
}
