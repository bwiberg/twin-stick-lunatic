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
    private int populationSize;

    private GeneticAlgorithm ga {
        get { return GeneticAlgorithm.Instance; }
    }

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
            StopPlaymode();
        }

        GUILayout.Label("Species", EditorStyles.boldLabel);
        speciesName = EditorGUILayout.DelayedTextField("Name");
        intelligence = EditorGUILayout.Slider("Intelligence", intelligence, 0.0f, 1.0f);
        populationSize = EditorGUILayout.IntSlider("Population size", populationSize, 1, 100);
        EditorGUI.EndDisabledGroup();
    }

    protected override void OnPlaymodeStarted(bool fromEditorPlayButton) {
        Debug.LogFormat("OnPlaymodeStarted({0}) from GeneticAlgorithmWindow",
            fromEditorPlayButton ? "EDITOR" : "CUSTOM");

        if (fromEditorPlayButton) {
            return;
        }

        ga.RunSingleGeneration(populationSize, () => {
            Debug.Log("Generation complete");
            StopPlaymode();
        });
        Repaint();
    }

    protected override void OnPlaymodeEnded(bool fromEditorPlayButton) {
        Debug.LogFormat("OnPlaymodeEnded({0}) from GeneticAlgorithmWindow", fromEditorPlayButton ? "EDITOR" : "CUSTOM");
        wantsToSimulate = false;

        Repaint();
    }
}
