using System.Collections;
using NeuralNetworks;
using UnityEngine;

namespace Genetics {
    public class Creature : CustomBehaviour {
        [SerializeField] private UpdateMethod UpdateMethod;
        [SerializeField] private RealtimeNeuralNet nn;

        public enum State {
            NotBorn,
            Alive,
            Dead
        }

        public State state { get; private set; }

        public int NumGenesRequired {
            get { return nn.RequiredWeights; }
        }

        private DNA dna;

        public DNA DNA {
            get { return dna; }
            set {
                dna = value;
                nn.Weights = dna.Genes;
            }
        }

        public void StartCreature() {
            nn.enabled = true;
        }

        public void EndCreature() {
            Destroy(gameObject);
        }
    }
}
