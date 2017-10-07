using System.Collections;
using UnityEngine;

namespace Genetics {
    public class Creature : CustomBehaviour {
        [SerializeField] private UpdateMethod UpdateMethod;
        
        public enum State {
            NotBorn,
            Alive,
            Dead
        }

        public State state { get; private set; }
        public DNA DNA { get; set; }
    }
}
