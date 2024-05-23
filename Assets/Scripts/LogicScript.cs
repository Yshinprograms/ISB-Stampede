using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Handles all UI, Spawning and logic in game

public class LogicScript : MonoBehaviour
{
    public int piperMaxHealth = 100;
    public int piperHealth;
    public HealthbarScript healthbar;

    // Import Game Manager Script & Game end conditions
    public GameManagerScript gameManager;
    private bool isDead = false;

    // Import GameObjects, drag and drop into Inspector
    public GameObject bollard;
    public GameObject paperBall;
    public GameObject freshie;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenBollardSpawn = 6f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;
    public float secondsBetweenFreshieSpawn = 6f;

    void Start()
    {
        // Bollard interaction and Spawns
        BollardScript.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating("spawnBollard", 0f, secondsBetweenBollardSpawn); // Calls spawnBollard every 6s from t=0

        // Piper's parameters & projectile interactions
        piperHealth = piperMaxHealth;
        healthbar.setMaxHealth(piperMaxHealth);
        PaperBallScript.activePaperBalls = 0;

        // Freshie interaction and Spawns + Future enemies
        FreshieScript.freshieCollisionEvent += freshieInflictDamage;
        InvokeRepeating("spawnFreshie", 0f, secondsBetweenFreshieSpawn); // Calls freshieBollard every 3s from t=0
    }


    void Update()
    {
        // Set minimum health to 0
<<<<<<< Updated upstream
        piperHealth = Mathf.Clamp(piperHealth, 0, 100);
        healthbar.setHealth(piperHealth);

        if (piperHealth <= 0 && !isDead)
        {
            isDead = true;
            gameManager.gameOver();
            Debug.Log("Dead");
        }


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
    void freshieInflictDamage()
    {
        piperHealth -= 20;
    }

    // Spawn Enemies
    void spawnBollard()
    {
        ObjectPoolScript.spawnObject(bollard, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void spawnFreshie()
    {
        ObjectPoolScript.spawnObject(freshie, SpawnScript.generateSpawnPoint(), Quaternion.identity);
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
        CancelInvoke("spawnFreshie");
        BollardScript.bollardCollisionEvent -= bollardInflictDamage;
        FreshieScript.freshieCollisionEvent -= freshieInflictDamage;

    }
}
