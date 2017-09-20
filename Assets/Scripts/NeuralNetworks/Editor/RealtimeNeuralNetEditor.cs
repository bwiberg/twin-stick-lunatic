using UnityEditor;
using NeuralNetworks;

[CustomEditor(typeof(RealtimeNeuralNet))]
public class RealtimeNeuralNetEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        RealtimeNeuralNet net = (RealtimeNeuralNet) target;
        EditorGUILayout.LabelField("Number of weights", net.RequiredWeights.ToString());
    }
}
