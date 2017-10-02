using UnityEditor;

public abstract class CustomEditorWindow : EditorWindow {
    protected enum Playmode {
        IsStopped,
        WantsToStartPlaying,
        IsPlaying,
        WantsToStopPlaying
    }

    protected Playmode playmode { get; private set; }

    private bool wasPlaying;

    protected void StartPlaymode() {
        if (playmode != Playmode.IsStopped) {
            return;
        }

        playmode = Playmode.WantsToStartPlaying;
        EditorApplication.isPlaying = true;
    }

    protected void StopPlaymode() {
        if (playmode != Playmode.IsPlaying) {
            return;
        }

        playmode = Playmode.WantsToStopPlaying;
        EditorApplication.isPlaying = false;
    }

    protected void Update() {
        bool isPlaying = EditorApplication.isPlaying;

        if (isPlaying && !wasPlaying) {
            OnPlaymodeStarted(playmode != Playmode.WantsToStartPlaying);
            playmode = Playmode.IsPlaying;
        }

        if (!isPlaying && wasPlaying) {
            OnPlaymodeEnded(playmode != Playmode.WantsToStopPlaying);
            playmode = Playmode.IsStopped;
        }

        wasPlaying = isPlaying;
    }

    protected abstract void OnPlaymodeStarted(bool fromEditorPlayButton);
    protected abstract void OnPlaymodeEnded(bool fromEditorPlayButton);
}
