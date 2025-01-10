using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class electricalTimelineManager : MonoBehaviour
{
    public PlayableDirector playableDirector; // Reference to the PlayableDirector
    public string Electrical; // Scene name to load after animation finishes

    private void Start()
    {
        // Start playing the Timeline as soon as the scene is loaded
        if (playableDirector != null)
        {
            playableDirector.Play();
            playableDirector.stopped += OnTimelineFinished; // Add listener to transition after the timeline finishes
        }
        else
        {
            Debug.LogError("PlayableDirector is not assigned.");
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        // Transition to the new scene after the Timeline finishes
        Debug.Log("Timeline finished. Transitioning to new scene...");
        SceneManager.LoadScene("Electrical");
    }
}
