using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles all audio in game, uses UI game object


public class AudioManager : MonoBehaviour
{
    // Just drag and drop audio sources into these
    public AudioSource paperBallCollisionSFX;
    public AudioSource bollardCollisionSFX;

    void Start()
    {
        // Subscribe to events
        BollardScript.bollardCollisionEvent += bollardSounds;
        PaperBallScript.paperBallCollisionEvent += paperBallSounds;
    }

    void bollardSounds()
    {
        bollardCollisionSFX.Play();
    }

    void paperBallSounds()
    {
        paperBallCollisionSFX.Play();
    }

    // Unsubscribe from events
    private void OnDestroy()
    {
        BollardScript.bollardCollisionEvent -= bollardSounds;
        PaperBallScript.paperBallCollisionEvent -= paperBallSounds;
    }
}
