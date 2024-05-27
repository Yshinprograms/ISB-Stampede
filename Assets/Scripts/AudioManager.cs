using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public AudioSource paperBallCollisionSFX;
    public AudioSource bollardCollisionSFX;
    public AudioSource paperBallThrownSFX;
    public AudioSource auntyCollisionSFX;
    public AudioSource handbagThrownSFX;
    public AudioSource cleanerEnrageSFX;

    void Start()
    {
        // Subscribe to events
        BollardScript.bollardCollisionEvent += bollardSounds;
        PaperBallScript.paperBallCollisionEvent += paperBallCollisionSounds;
        PaperBallScript.paperBallThrownEvent += paperBallThrownSounds;
        AuntyScript.auntyCollisionEvent += auntyCollisionSounds;
        AuntyScript.auntyThrowEvent += handbagThrownSounds;
        CleanerScript.cleanerEnrageEvent += cleanerEnrageSounds;
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
    void auntyCollisionSounds()
    {
        auntyCollisionSFX.Play();
    }
    void handbagThrownSounds()
    {
        handbagThrownSFX.Play();
    }
    void cleanerEnrageSounds()
    {
        cleanerEnrageSFX.Play();
    }

    // Unsubscribe from events
    private void OnDestroy()
    {
        BollardScript.bollardCollisionEvent -= bollardSounds;
        PaperBallScript.paperBallCollisionEvent -= paperBallCollisionSounds;
        PaperBallScript.paperBallThrownEvent -= paperBallThrownSounds;
        AuntyScript.auntyCollisionEvent -= auntyCollisionSounds;
        AuntyScript.auntyThrowEvent -= handbagThrownSounds;
        CleanerScript.cleanerEnrageEvent -= cleanerEnrageSounds;
    }
}
