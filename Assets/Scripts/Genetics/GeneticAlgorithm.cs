using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KdTree;
using Logging;
using UnityEngine;
using Utility;

namespace Genetics {
    public class GeneticAlgorithm : CustomSingletonBehaviour<GeneticAlgorithm> {
        public delegate void GenerationComplete(int generation,
            float duration,
            SortedList<float, DNA> dnasByFitness,
            SortedList<float, DNA> hallOfFame);

        #region STATIC_FIELDS

        private static readonly DuplicateKeyComparer<float> Comparer =
            new DuplicateKeyComparer<float>(SequenceOrdering.Reverse);

        public static uint HallOfFameCount = 100;

        #endregion


        #region SERIALIZABLE_FIELDS

        [SerializeField] private FitnessEvaluator FitnessEvaluator;
        [SerializeField, Range(0, 1)] private float FractionFromHallOfFame = 0.25f;
        [SerializeField] private UpdateMethod UpdateMethod;
        [SerializeField, Range(1.0f, 120.0f)] private float TerminationTime = 10.0f;
        [SerializeField] private CreatureCreator CreatureCreator;

        #endregion


        #region PRIVATE_VARIABLES

        private readonly SortedList<float, DNA> HallOfFame = new SortedList<float, DNA>(
            Mathf.RoundToInt(1.3f * HallOfFameCount), Comparer);
        private SortedList<float, DNA> PreviousGenerationDnasByFitness;
        private Vector3 resetPlayerPosition;
        private Quaternion resetPlayerQuaternion;

        #endregion


        #region UNITY_EVENT_METHODS

        #endregion

        #region PUBLIC_PROPERTIES

        public int NumGenesRequired {
            get { return CreatureCreator.NumGenesRequired; }
        }

        #endregion

        public void Run(int populationSize, GenerationComplete callback, int numRuns) {
            resetPlayerPosition = player.transform.position;
            resetPlayerQuaternion = player.transform.rotation;
            StartCoroutine(RunMultipleGenerations(populationSize, callback, numRuns));
        }

        private IEnumerator RunMultipleGenerations(int populationSize, GenerationComplete callback, int numGenerations) {
            for (int gen = 1; gen <= numGenerations; ++gen) {
                yield return StartCoroutine(RunSingleGeneration(populationSize, gen, callback));
                FileLogger.Log("\n");
            }
        }

        private IEnumerator RunSingleGeneration(int populationSize,
            int generation,
            GenerationComplete callback) {
            FileLogger.Log("==== Generation {0}, N={1}", generation, populationSize);

            Creature[] population = CreatePopulation(populationSize, generation);

            // reset player
            player.transform.position = resetPlayerPosition;
            player.transform.rotation = resetPlayerQuaternion;

            // simulation loop
            float startTime = Time.time;
            do {
                yield return WaitForNextUpdate();
            } while (Time.time - startTime < TerminationTime &&
                     population.Select(t => t)
                         .Any(t => t.state != Creature.State.Dead));


            float duration = Time.time - startTime;
            FileLogger.Log("  completed in {1}s", generation, duration);

            // evaluate fitness of population
            float[] fitnesses = population
                .Select(t => FitnessEvaluator.ComputeForCreature(t))
                .ToArray();

            // keep generation
            PreviousGenerationDnasByFitness = new SortedList<float, DNA>(populationSize, Comparer);
            for (int i = 0; i < populationSize; i++) {
                PreviousGenerationDnasByFitness.Add(fitnesses[i], population[i].DNA);

                // also add to Hall of Fame
                HallOfFame.Add(fitnesses[i], population[i].DNA);
            }

            // add to Hall of Fame
            HallOfFame.KeepFirstN(HallOfFameCount);

            FileLogger.Log("  population fitness evaluation;");
            PreviousGenerationDnasByFitness.ForEach((kvp, i) => {
                    FileLogger.Log("  {0}. [fitness={1}, dna={2}]", i + 1, kvp.Key, kvp.Value);
                }
            );

            callback(generation, duration, PreviousGenerationDnasByFitness, HallOfFame);

            FileLogger.Log("==== Generation {0} complete", generation);

            // end all creatures
            population.ForEach(t => t.EndCreature());
        }

        private Creature[] CreatePopulation(int populationSize, int generation) {
            var dnas = CreateDNAs(populationSize, generation);

            var population = new Creature[populationSize];
            for (int i = 0; i < populationSize; i++) {
                population[i] = CreatureCreator.Create(dnas[i], i);
            }

            return population;
        }

        private DNA[] CreateDNAs(int populationSize, int generation) {
            if (generation == 1) {
                return new DNA[populationSize]
                    .Select(t => new DNA((uint) CreatureCreator.NumGenesRequired))
                    .ToArray();
            }
            var numFromHallOfFame = Mathf.RoundToInt(populationSize * FractionFromHallOfFame);

            // 1) select DNAs to breed new population from
            var parentDNAs = new DNA[populationSize];

            // 1a) add some DNA from Hall of Fame
            int i;
            for (i = 0; i < HallOfFame.Count && i < numFromHallOfFame; i++) {
                parentDNAs[i] = HallOfFame.Values[i];
            }

            // 1b) add some DNA from previous generation, based on fitness
            for (; i < populationSize; ++i) {
                parentDNAs[i] =
                    PreviousGenerationDnasByFitness.GetRandomDistributedByKey(
                        new DNA((uint) CreatureCreator.NumGenesRequired)).Item2;
            }

            FileLogger.Log("  parentDNAs: [{0}]", parentDNAs
                .Select(t => t.ToString())
                .Aggregate((s, t) => string.Format("{0}, {1}", s, t)));

            // 2) breed new population
            var dnas = new DNA[populationSize];

            // 2a) add some of the best DNA from the hall of fame directly
            for (i = 0; i < numFromHallOfFame; ++i) {
                dnas[i] = HallOfFame.Values[i];
            }

            // 2b) breed the rest of the DNA from the prev. selected parent DNAs
            for (; i < populationSize; i += 2) {
                var parents = parentDNAs.GetRandom(2);
                var tuple = DNA.Breed(parents[0], parents[1]);
                dnas[i] = tuple.Item1;
                if (i + 1 < populationSize) {
                    dnas[i + 1] = tuple.Item2;
                }
            }

            FileLogger.Log("  population: [{0}]", dnas
                .Select(t => t.ToString())
                .Aggregate((s, t) => string.Format("{0}, {1}", s, t)));

            return dnas;
        }

        private YieldInstruction WaitForNextUpdate() {
            if (UpdateMethod == UpdateMethod.FixedUpdate) {
                return new WaitForFixedUpdate();
            }
            return null;
        }
    }
}
