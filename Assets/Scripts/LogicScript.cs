using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Handles all UI, Spawning and logic in game

public class LogicScript : MonoBehaviour
{
    public int piperHealth = 100;
    public Text healthbar;

    // Import GameObjects, drag and drop into Inspector
    public GameObject bollard;
    public GameObject paperBall;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenBollardSpawn = 6f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;

    void Start()
    {
        // Bollard interaction and Spawns
        BollardScript.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating("spawnBollard", 0f, secondsBetweenBollardSpawn); // Calls spawnBollard every 6s from t=0

        // Piper's projectile interactions
        PaperBallScript.activePaperBalls = 0;

        // Freshie interaction and Spawns + Future enemies
    }


    void Update()
    {
        healthbar.text = piperHealth.ToString();

        // Piper's projectile interactions
        secondsBetweenPaperBallSpawn += Time.deltaTime;
        if (PaperBallScript.activePaperBalls < maxActivePaperBalls && secondsBetweenPaperBallSpawn > 1)
        {
            spawnPaperBall();
            secondsBetweenPaperBallSpawn = 0;
            PaperBallScript.activePaperBalls += 1;
        }
    }

    // Damages
    void bollardInflictDamage()
    {
        piperHealth -= 10;
    }

    // Spawn Enemies
    void spawnBollard()
    {
        ObjectPoolScript.spawnObject(bollard, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }

    // Spawn Projectiles
    void spawnPaperBall()
    {
        // Spawn to the right of Piper
        ObjectPoolScript.spawnObject(paperBall, PiperScript.piperPosition + Vector3.right, Quaternion.identity);
    }

    // Unsubscribe from Events
    private void OnDestroy()
    {
        CancelInvoke("spawnBollard");
        BollardScript.bollardCollisionEvent -= bollardInflictDamage;
    }
}
