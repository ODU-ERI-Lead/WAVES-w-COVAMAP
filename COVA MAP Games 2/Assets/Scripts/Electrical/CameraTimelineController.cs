using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CameraTimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Camera Camera1;
    public Camera Camera2;
    public float switchTime = 5f;
    public float moveSpeed = 5f;
    public Vector3 moveDirection = new Vector3(2f,0f,0f);

    private bool TimelineFinished = false;



    private void Start()
    {
        Camera1.gameObject.SetActive(true);
        Camera2.gameObject.SetActive(false);

        if (playableDirector != null )
        {
            playableDirector.Play();
        }
    }

    private void Update()
    {
        if (playableDirector != null && playableDirector.state == PlayState.Playing)
        {
            MoveCamera1();
        }
        if (playableDirector !=null && playableDirector.state == PlayState.Paused && !TimelineFinished)
        {
            TimelineFinished = true;

            SwitchToCamera2();
        }
    }

    void MoveCamera1()
    {
        Camera1.transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void SwitchToCamera2()
    {
        Camera1.gameObject.SetActive(false);
        Camera2.gameObject.SetActive(true);

    }


}
