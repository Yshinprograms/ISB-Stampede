using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Handles all audio in game, uses UI game object

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    public AudioSource paperBallCollisionSFX;
    public AudioSource bollardCollisionSFX;
    public AudioSource paperBallThrownSFX;
    public AudioSource freshieCollisionSFX;
    public AudioSource csMuggerCollisionSFX;
    public AudioSource csMuggerCodeCollisionSFX;

    [Header("---------- Audio Clip ----------")]
    // Just drag and drop wav files into Audio Sources in inspector
    // Then drag the audio source into Audio Manager script in inspector
    public AudioClip background;

    void Start()
    {
        // Plays background Music for Level 1
        musicSource.clip = background;
        musicSource.Play();

        // Subscribe to events
        BollardScript.bollardCollisionEvent += bollardSounds;
        PaperBallScript.paperBallCollisionEvent += paperBallCollisionSounds;
        PaperBallScript.paperBallThrownEvent += paperBallThrownSounds;
        FreshieScript.freshieCollisionEvent += freshieSounds;
        CSMuggerScript.csMuggerCollisionEvent += csMuggerSounds;
        csMuggerCodeSpawnScript.csMuggerCodeCollisionEvent += csMuggerCodeSounds;
    }

    void freshieSounds()
    {
        freshieCollisionSFX.Play();
    }

    void bollardSounds()
    {
        bollardCollisionSFX.Play();
    }

    void paperBallCollisionSounds()
    {
        paperBallCollisionSFX.Play();
    }
    void paperBallThrownSounds()
    {
        paperBallThrownSFX.Play();
    }

    void csMuggerSounds()
    {
        csMuggerCollisionSFX.Play();
    }

    void csMuggerCodeSounds()
    {
        csMuggerCodeCollisionSFX.Play();
    }

    // Unsubscribe from events
    private void OnDestroy()
    {
        BollardScript.bollardCollisionEvent -= bollardSounds;
        FreshieScript.freshieCollisionEvent -= freshieSounds;
        PaperBallScript.paperBallCollisionEvent -= paperBallCollisionSounds;
        PaperBallScript.paperBallThrownEvent -= paperBallThrownSounds;
        CSMuggerScript.csMuggerCollisionEvent -= csMuggerSounds;
        csMuggerCodeSpawnScript.csMuggerCodeCollisionEvent -= csMuggerCodeSounds;
        
    }
}
