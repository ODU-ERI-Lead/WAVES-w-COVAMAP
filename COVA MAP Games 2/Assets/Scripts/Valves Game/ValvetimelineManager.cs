using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class ValvetimelineManager : MonoBehaviour
{
    public string Valves; //name of scene to be loaded after timeline
    public PlayableDirector playableDirector; //timeline controller 

    private void Start()
    {
        if (playableDirector == null)
        {
            playableDirector.Play();

            playableDirector.stopped += OnTimelineFinished;
        }
        else
        {
            Debug.LogError("PlayableDirector is not assigned");
        }
    }

    private void OnTimelineFinished(PlayableDirector playableDirector)
    {
        playableDirector.stopped -= OnTimelineFinished;

        LoadNewScene();

    }

    private void LoadNewScene()
    {
        SceneManager.LoadSceneAsync("Valves");
    }

}
