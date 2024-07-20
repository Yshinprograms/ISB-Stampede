using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * List of AudioSources:
 * -----------Defaults-----------
 * Piper
 *   - paperBallCollision
 * Mala
 * PiperFriend
 * -----------Level 1-----------
 * Bollard
 *   - bollardCollision
 * Freshie
 *   - freshieCollision
 * Aunty
 *   - auntyCollision
 *   - handbagThrown
 * L1Boss
 * -----------Level 2-----------
 * Cleaner
 *   - cleanerEnrage
 * EnginKid
 *   - enginKidGathered
 * CSMugger
 *   - csMuggerCollision
 *   - csMuggerCodeCollision
 * CS1010
 * -----------Level 3-----------
 * MedStudent
 * InnocentStudent
 * BizSnake
 * ChineseTourist
 * ChineseTourBus
 * 
 * To-do:
 * Mala
 * PiperFriend
 * CS1010
 * InnocentStudent
 * BizSnake
 * CT
 * CTBus
 */

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

    public AudioSource malaSFX;
    public AudioSource friendSpawnSFX;
    public AudioSource friendPaperThrownSFX;
    public AudioSource cs1010SFX;
    public AudioSource pe0SFX;
    public AudioSource pe1SFX;
    public AudioSource pe2SFX;
    public AudioSource cs1010ShootSFX;
    public AudioSource innocentStudentLatchOnSFX;
    public AudioSource bizSnakeTransformSFX;
    public AudioSource landmarkChangeSFX;
    public AudioSource chineseTourBusMoveSFX;
    public AudioSource chineseTouristFlashSFX;

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
        L2LogicScript.EnginKidGatheredEvent += enginKidGatheredSounds;

        MalaScript.MalaActiveEvent += MalaSounds;
        FriendScript.FriendSpawnEvent += FriendSounds;
        FriendScript.FriendPaperSpawnEvent += FriendPaperSounds;
        CS1010Script.CS1010SpawnEvent += CS1010Sounds;
        CS1010Script.PE0SpawnEvent += PE0Sounds;
        CS1010Script.PE1SpawnEvent += PE1Sounds;
        CS1010Script.PE2SpawnEvent += PE2Sounds;
        CS1010Script.CS1010ShootEvent += CS1010ShootSounds;
        InnocentStudentScript.ISLatchOnEvent += InnocentStudentSounds;
        BizSnake.TransformEvent += BizSnakeSounds;
        ChineseTourist.PhotoEvent += CTFlashSounds;
        L3LogicScript.LandmarkChangeEvent += LandmarkChangeSounds;
        ChineseTourBusScript.BusMoveEvent += CTBusMovingSounds;
    }

    void MalaSounds()
    {
        malaSFX.Play();
    }
    void FriendSounds()
    {
        friendSpawnSFX.Play();
    }
    void FriendPaperSounds()
    {
        friendPaperThrownSFX.Play();
    }
    void CS1010Sounds()
    {
        cs1010SFX.Play();
    }
    void PE0Sounds()
    {
        pe0SFX.Play();
    }
    void PE1Sounds()
    {
        pe1SFX.Play();

    }
    void PE2Sounds()
    {
        pe2SFX.Play();
    }
    void CS1010ShootSounds()
    {
        cs1010ShootSFX.Play();
    }
    void InnocentStudentSounds()
    {
        innocentStudentLatchOnSFX.Play();
    }
    void BizSnakeSounds()
    {
        bizSnakeTransformSFX.Play();

    }
    void LandmarkChangeSounds()
    {
        landmarkChangeSFX.Play();
    }
    void CTBusMovingSounds()
    {
        chineseTourBusMoveSFX.Play();

    }
    void CTFlashSounds()
    {
        chineseTouristFlashSFX.Play();
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
        L2LogicScript.EnginKidGatheredEvent -= enginKidGatheredSounds;

        MalaScript.MalaActiveEvent -= MalaSounds;
        FriendScript.FriendSpawnEvent -= FriendSounds;
        FriendScript.FriendPaperSpawnEvent -= FriendPaperSounds;
        CS1010Script.CS1010SpawnEvent -= CS1010Sounds;
        CS1010Script.PE0SpawnEvent -= PE0Sounds;
        CS1010Script.PE1SpawnEvent -= PE1Sounds;
        CS1010Script.PE2SpawnEvent -= PE2Sounds;
        CS1010Script.CS1010ShootEvent -= CS1010ShootSounds;
        InnocentStudentScript.ISLatchOnEvent -= InnocentStudentSounds;
        BizSnake.TransformEvent -= BizSnakeSounds;
        ChineseTourist.PhotoEvent -= CTFlashSounds;
        L3LogicScript.LandmarkChangeEvent -= LandmarkChangeSounds;
        ChineseTourBusScript.BusMoveEvent -= CTBusMovingSounds;
    }
}
