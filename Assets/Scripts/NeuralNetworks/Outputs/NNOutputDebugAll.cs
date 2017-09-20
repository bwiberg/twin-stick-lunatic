using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NeuralNetworks {
    namespace Outputs {
        public class NNOutputDebugAll : RealtimeNeuralNetOutput {
            [SerializeField] private int NumberOfOutputs = 1;
            
            public override int OutputCount {
                get { return NumberOfOutputs; }
            }

            public override void HandleOutput(float[] values, int offset) {
                StringBuilder sb = new StringBuilder("Output: [");
                for (int i = offset; i < offset + OutputCount - 1; ++i) {
                    sb.AppendFormat("{0}, ", values[i]);
                }
                sb.AppendFormat("{0}]", values[offset + OutputCount - 1]);
                Debug.Log(sb.ToString());
            }
        }
    }
}
