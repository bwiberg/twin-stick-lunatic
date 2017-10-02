using System.Collections;
using System.Collections.Generic;
using Genetics;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GeneticAlgorithmWindow : CustomEditorWindow {
    private bool wantsToSimulate;

    private string speciesName;
    private float intelligence;

    private GeneticAlgorithm geneticAlgorithm;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Genetic Algorithm")]
    public static void ShowWindow() {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(GeneticAlgorithmWindow));
    }
    
    private void OnGUI() {
        EditorGUI.BeginDisabledGroup(playmode == Playmode.IsPlaying);

        if (playmode == Playmode.IsStopped && GUILayout.Button("Start evolution")) {
            wantsToSimulate = true;
            StartPlaymode();
        }
        else if (playmode == Playmode.IsPlaying && wantsToSimulate && GUILayout.Button("End evolution")) {
            geneticAlgorithm.SaveResults();
            StopPlaymode();
        }

        GUILayout.Label("Species", EditorStyles.boldLabel);
        speciesName = EditorGUILayout.DelayedTextField("Name");
        intelligence = EditorGUILayout.Slider("Intelligence", intelligence, 0.0f, 1.0f);
        EditorGUI.EndDisabledGroup();
    }

    protected override void OnPlaymodeStarted(bool fromEditorPlayButton) {
        Debug.LogFormat("OnPlaymodeStarted({0}) from GeneticAlgorithmWindow",
            fromEditorPlayButton ? "EDITOR" : "CUSTOM");

        var gameObject = new GameObject("Genetic Algorithm (Singleton)");
        geneticAlgorithm = gameObject.AddComponent<GeneticAlgorithm>();

        Debug.LogFormat("Number of CustomBehaviours: {0}", FindObjectsOfType<CustomBehaviour>().Length);
        
        Repaint();
    }

    protected override void OnPlaymodeEnded(bool fromEditorPlayButton) {
        Debug.LogFormat("OnPlaymodeEnded({0}) from GeneticAlgorithmWindow", fromEditorPlayButton ? "EDITOR" : "CUSTOM");
        wantsToSimulate = false;
        
        Repaint();
    }
}
