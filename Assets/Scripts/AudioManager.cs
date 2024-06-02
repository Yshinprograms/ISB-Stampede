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

    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    public AudioSource paperBallCollisionSFX;
    public AudioSource bollardCollisionSFX;
    public AudioSource paperBallThrownSFX;
    public AudioSource freshieCollisionSFX;
    public AudioSource csMuggerCollisionSFX;
    public AudioSource csMuggerCodeCollisionSFX;
    public AudioSource auntyCollisionSFX;
    public AudioSource handbagThrownSFX;
    public AudioSource cleanerEnrageSFX;
    public AudioSource enginKidGatheredSFX;

    [Header("---------- Audio Clip ----------")]
    // Just drag and drop wav files into Audio Sources in inspector
    // Then drag the audio source into Audio Manager script in inspector
    public AudioClip background;

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

        // Make sure the AudioManager persists between scenes
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Plays background Music for Level 1
        musicSource.clip = background;
        musicSource.Play();

        // Subscribe to events
        Bollard.bollardCollisionEvent += bollardSounds;
        PaperBallScript.paperBallCollisionEvent += paperBallCollisionSounds;
        PaperBallScript.paperBallThrownEvent += paperBallThrownSounds;
        Freshie.freshieCollisionEvent += freshieSounds;
        CSMugger.csMuggerCollisionEvent += csMuggerSounds;
        CSMuggerCode.csMuggerCodeCollisionEvent += csMuggerCodeSounds;
        Aunty.auntyCollisionEvent += auntyCollisionSounds;
        Aunty.auntyThrowEvent += handbagThrownSounds;
        Cleaner.cleanerEnrageEvent += cleanerEnrageSounds;
        LogicScript.enginKidGatheredEvent += enginKidGatheredSounds;
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
    void enginKidGatheredSounds()
    {
        enginKidGatheredSFX.Play();
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
        Bollard.bollardCollisionEvent -= bollardSounds;
        Freshie.freshieCollisionEvent -= freshieSounds;
        PaperBallScript.paperBallCollisionEvent -= paperBallCollisionSounds;
        PaperBallScript.paperBallThrownEvent -= paperBallThrownSounds;
        CSMugger.csMuggerCollisionEvent -= csMuggerSounds;
        CSMuggerCode.csMuggerCodeCollisionEvent -= csMuggerCodeSounds;
        Aunty.auntyCollisionEvent -= auntyCollisionSounds;
        Aunty.auntyThrowEvent -= handbagThrownSounds;
        Cleaner.cleanerEnrageEvent -= cleanerEnrageSounds;
        LogicScript.enginKidGatheredEvent -= enginKidGatheredSounds;
    }
}
