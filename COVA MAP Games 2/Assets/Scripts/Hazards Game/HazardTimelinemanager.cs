using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class HazardTimelinemanager: MonoBehaviour
{
    public string hazards; // The name of the scene to load after the timeline finishes.
    public PlayableDirector playableDirector; // The PlayableDirector that controls the Timeline.

    void Start()
    {
        // Make sure PlayableDirector is set
        if (playableDirector != null)
        {
            // Play the timeline
            playableDirector.Play();

            // Subscribe to the timeline's finished event
            playableDirector.stopped += OnTimelineFinished;
        }
        else
        {
            Debug.LogError("PlayableDirector is not assigned.");
        }
    }

    // This will be called when the timeline finishes playing
    private void OnTimelineFinished(PlayableDirector director)
    {
        // Unsubscribe from the event to avoid repeated calls
        playableDirector.stopped -= OnTimelineFinished;

        // Load the new scene after the timeline finishes
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        // Load the new scene asynchronously to avoid freezing the game
        SceneManager.LoadSceneAsync("hazards");
    }
}
