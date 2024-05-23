using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all audio in game, uses UI game object

public class AudioManager : MonoBehaviour
{
    // Just drag and drop wav files into Audio Sources in inspector
    // Then drag the audio source into Audio Manager script in inspector
    public AudioSource paperBallCollisionSFX;
    public AudioSource bollardCollisionSFX;
    public AudioSource paperBallThrownSFX;
    public AudioSource handbagThrownSFX;

    void Start()
    {
        // Subscribe to events
        BollardScript.bollardCollisionEvent += bollardSounds;
        PaperBallScript.paperBallCollisionEvent += paperBallCollisionSounds;
        PaperBallScript.paperBallThrownEvent += paperBallThrownSounds;
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

    // Unsubscribe from events
    private void OnDestroy()
    {
        BollardScript.bollardCollisionEvent -= bollardSounds;
        PaperBallScript.paperBallCollisionEvent -= paperBallCollisionSounds;
        PaperBallScript.paperBallThrownEvent -= paperBallThrownSounds;
    }
}
