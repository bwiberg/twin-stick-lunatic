using System;
using System.Collections;
using System.Collections.Generic;
using KdTree;
using UnityEngine;

namespace Genetics {
    public class GeneticAlgorithm : CustomSingletonBehaviour<GeneticAlgorithm> {
        public static int HallOfFameCount = 100;
        
        [SerializeField] private FitnessEvaluator FitnessEvaluator;
        [SerializeField, Range(1, 100)] private int GeneCount = 10;
        
        private readonly SortedList<DNA, float> HallOfFame = new SortedList<DNA, float>(HallOfFameCount);

        public IEnumerator RunSingleGeneration(int populationSize) {
            
        }
        
        public void SaveResults() {
            Debug.Log("Saving results...");
        }
    }
}
