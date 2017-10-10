using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NeuralNetworks.Outputs {
    public class NNOutputDebugAll : RealtimeNeuralNetOutput {
        [SerializeField] private int NumberOfOutputs = 0;

        public override int OutputCount {
            get { return NumberOfOutputs; }
        }

        public override void HandleOutput(float[] values, int offset) {
            var sb = new StringBuilder("Output: [");
            foreach (float t in values) {
                sb.AppendFormat("{0}, ", t);
            }
            sb.AppendFormat("{0}]", values[offset + OutputCount - 1]);
            Debug.Log(sb.ToString());
        }
    }
}
