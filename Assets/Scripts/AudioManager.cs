using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/*  Handles all audio in game
 *  Guide to adding audio:
 * 1. Find and download your audio file
 * 2. Create a delegate event type(s) in the respective gameObject script
 * 3. Place that event(s) wherever you want your audio to be played
 * 4. Go to AudioManager(here), create AudioSource component
 * 5. Add an Audio Source component to AudioManager in the inspector
 * 6. Drag your audio file into the Audio Sources in inspector
 * 7. Then drag the audio source into Audio Manager script in inspector
 * 8. Subscribe to your event within AudioManagerScript
 * 9. Remember to Unsubscribe
 */

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager Instance { get; private set; }
    //public static AudioManager Instance = null;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Plays background Music for Main Menu
        //musicSource.Play();
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume (float volume)
    {
        sfxSource.volume = volume;
    }

}
