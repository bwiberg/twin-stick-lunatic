using UnityEditor;
using NeuralNetworks;

namespace NeuralNetworks.Editor {
    [CustomEditor(typeof(RealtimeNeuralNet))]
    public class RealtimeNeuralNetEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            RealtimeNeuralNet net = (RealtimeNeuralNet) target;
            EditorGUILayout.LabelField("Number of weights", net.RequiredWeights.ToString());
        }
    }
}
