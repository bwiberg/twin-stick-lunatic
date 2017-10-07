using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KdTree;
using UnityEngine;
using Utility;

namespace Genetics {
    public class GeneticAlgorithm : CustomSingletonBehaviour<GeneticAlgorithm> {
        public delegate void GenerationComplete();

        public static int HallOfFameCount = 100;
        private static readonly DuplicateKeyComparer<float> Comparer = new DuplicateKeyComparer<float>();

        [SerializeField] private UpdateMethod UpdateMethod;

        [SerializeField, Range(1.0f, 120.0f)] private float TerminationTime = 10.0f;
        [SerializeField] private FitnessEvaluator FitnessEvaluator;
        [SerializeField, Range(1, 100)] private int GeneCount = 10;

        private readonly SortedList<float, DNA> HallOfFame = new SortedList<float, DNA>(
            Mathf.RoundToInt(1.3f * HallOfFameCount), Comparer);

        private SortedList<float, DNA> PreviousGenerationDnasByFitness;

        public int Generation { get; private set; }

        public IEnumerator RunSingleGeneration(int populationSize,
            GenerationComplete callback) {
            Creature[] population = new Creature[populationSize];

            float startTime = Time.time;

            // simulation loop
            do {
                yield return WaitForNextUpdate();
            } while (Time.time - startTime < TerminationTime || population
                         .Select(t => t)
                         .Any(t => t.state != Creature.State.Dead));

            // evaluate fitness of population
            float[] fitnesses = population
                .Select(t => FitnessEvaluator.ComputeForCreature(t))
                .ToArray();

            // keep generation
            PreviousGeneration = new SortedList<float, DNA>(populationSize, Comparer);
            for (int i = 0; i < populationSize; i++) {
                PreviousGeneration.Add(fitnesses[i], population[i].DNA);

                // also add to Hall of Fame
                HallOfFame.Add(fitnesses[i], population[i].DNA);
            }

            // add to Hall of Fame
            HallOfFame.KeepFirstN(HallOfFameCount);

            callback();
        }

        private Creature[] CreateGeneration(int populationSize) {
            
        }

        private YieldInstruction WaitForNextUpdate() {
            if (UpdateMethod == UpdateMethod.FixedUpdate) {
                return new WaitForFixedUpdate();
            }
            return null;
        }
    }
}
