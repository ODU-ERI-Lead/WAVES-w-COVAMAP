using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


public class Mutebutton : MonoBehaviour
{
   
   public AudioListener audioListener;
   public List<AudioSource> audioSources = new List<AudioSource>();
    public Sprite mute;
    public Sprite Nomute;
    public Button mutebutton;
    public bool beenmuted = false;   // may need constant or something to have mute and mute unmute work properly, I can get the image to update correct but then there never is any volume.

    void Start()
    {
        // Add an onClick listener to the mute button
        mutebutton.onClick.AddListener(ToggleMute);
        UpdateButtonImage();
    }

     public void ToggleMute()
    {
        // Toggle the mute state
        beenmuted = !beenmuted;
    
        // Set the audio state to mute/unmute
        AudioListener.volume = beenmuted ? 0f : 1f;

        // Change the button image based on the mute state
        UpdateButtonImage();
    }

    public void AddAudioToList(AudioSource audiosource)
    {
        if (!audioSources.Contains(audiosource))
        {
            audioSources.Add(audiosource);
        }
    }

    public void UpdateButtonImage()
    {
        mutebutton.image.sprite = beenmuted ? mute : Nomute;
        if (beenmuted = false )
        {
           // AudioListener.volume = 1f;
           // AudioListener.GetComponent(audioSources).volume
           // test remove // above and below other iff statement if needed
            //Button.overrideSprite = mute;
        }
        if (beenmuted = true )
        {
            //AudioListener.volume = 0f;
        }
    }
    public void MuteAll()
    {
        foreach (AudioSource source in audioSources)
        {
            source.mute = true;
            mutebutton.image.sprite = mute;
        }
    }

    // Unmute all audio sources in the list
    public void UnmuteAll()
    {
        foreach (AudioSource source in audioSources)
        {
            source.mute = false;
            mutebutton.image.sprite = Nomute;
        }
    }

}
