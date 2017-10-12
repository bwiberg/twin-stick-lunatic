using System.Collections;
using System.Collections.Generic;
using Genetics;
using Logging;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GeneticAlgorithmWindow : CustomEditorWindow {
    private bool wantsToSimulate;
    private string logFilePath;

    private string speciesName;
    private int numGenerations;
    private int populationSize;
    private float mutationMax;

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
        if (playmode == Playmode.IsStopped && GUILayout.Button("Start evolution")) {
            wantsToSimulate = true;
            StartPlaymode();
            Random.InitState(System.DateTime.Now.Millisecond);
        }
        else if (playmode == Playmode.IsPlaying && wantsToSimulate && GUILayout.Button("End evolution")) {
            StopPlaymode();
        }

        EditorGUI.BeginDisabledGroup(playmode == Playmode.IsPlaying);
        logFilePath = Application.dataPath + EditorGUILayout.DelayedTextField("Path to log file", "/Resources/log.txt");
        if (GUILayout.Button("Clear log file")) {
            FileLogger.ClearFile();
        }
        EditorGUI.EndDisabledGroup();
        FileLogger.FilePath = logFilePath;
        
        EditorGUI.BeginDisabledGroup(playmode == Playmode.IsPlaying);
        GUILayout.Label("Species", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Number of genes", ga.NumGenesRequired.ToString());
        speciesName = EditorGUILayout.DelayedTextField("Name", "<placeholder>");
        numGenerations = EditorGUILayout.IntSlider("Number of generations", numGenerations, 1, 1000);
        populationSize = EditorGUILayout.IntSlider("Population size", populationSize, 1, 100);
        mutationMax = EditorGUILayout.Slider("Mutation max", mutationMax, 0, 1.0f);
        EditorGUI.EndDisabledGroup();
    }

    protected override void OnPlaymodeStarted(bool fromEditorPlayButton) {
        Debug.LogFormat("OnPlaymodeStarted({0}) from GeneticAlgorithmWindow",
            fromEditorPlayButton ? "EDITOR" : "CUSTOM");

        if (fromEditorPlayButton) {
            return;
        }

        ga.MutationMax = mutationMax;
        ga.Run(populationSize, (generation, duration, dnasByFitness, hallOfFame) => {
            //Debug.LogFormat("Generation #{0} completed in {1}s", generation, duration);
            Repaint();

            if (generation == numGenerations) {
                StopPlaymode();
            }
        }, numGenerations);
        Repaint();
    }

    protected override void OnPlaymodeEnded(bool fromEditorPlayButton) {
        Debug.LogFormat("OnPlaymodeEnded({0}) from GeneticAlgorithmWindow", fromEditorPlayButton ? "EDITOR" : "CUSTOM");
        wantsToSimulate = false;

        Repaint();
    }
}
