using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSounds : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
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

    void Start()
    {

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
