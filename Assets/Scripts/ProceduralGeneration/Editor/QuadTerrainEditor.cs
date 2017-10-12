using UnityEditor;
using UnityEngine;

namespace ProceduralGeneration.Editor {
    [CustomEditor(typeof(QuadTerrain))]
    public class QuadTerrainEditor : UnityEditor.Editor {
        private QuadTerrain Target {
            get { return (QuadTerrain) target; }
        }

        private bool randomizeOffset;

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            GUILayout.Label("Generation", EditorStyles.boldLabel);
            randomizeOffset = EditorGUILayout.Toggle("Randomize offset", randomizeOffset);
            if (GUILayout.Button("Generate")) {
                if (randomizeOffset) {
                    Target.NormalizedOffset = new Vector2(Random.value, Random.value);
                }
                
                Target.TimeOfGeneration = QuadTerrain.GenerationTime.EditorOnly;
                Target.UpdateTerrain();
            }
        }
    }
}
