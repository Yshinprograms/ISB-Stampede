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
    private bool isAlive;

    // Import GameObjects, drag and drop into Inspector
    public GameObject bollard;
    public GameObject paperBall;
    public GameObject freshie;
    public GameObject aunty;
    public GameObject cleaner;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenBollardSpawn = 6f;
    public float secondsBetweenFreshieSpawn = 6f;
    public float secondsBetweenAuntySpawn = 6f;
    public float secondsBetweenCleanerSpawn = 5f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;

    void Start()
    {

        // Piper's parameters & projectile interactions
        isAlive = true;
        piperHealth = piperMaxHealth;
        healthbar.setMaxHealth(piperMaxHealth);
        PaperBallScript.activePaperBalls = 0;

        // Bollard interaction and Spawns
        BollardScript.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating("spawnBollard", 0f, secondsBetweenBollardSpawn);

        // Freshie interaction and Spawns
        FreshieScript.freshieCollisionEvent += freshieInflictDamage;
        InvokeRepeating("spawnFreshie", 1110f, secondsBetweenFreshieSpawn);

        // Aunty interactions and spawns
        AuntyScript.auntyCollisionEvent += auntyInflictDamage;
        HandbagScript.handbagCollisionEvent += handbagInflictDamage;
        InvokeRepeating("spawnAunty", 0f, secondsBetweenAuntySpawn);

        // Cleaner interaction and Spawns
        CleanerScript.cleanerCollisionEvent += cleanerInflictDamage;
        InvokeRepeating("spawnCleaner", 0f, secondsBetweenFreshieSpawn);
    }


    void Update()
    {
        // Set minimum health to 0
        healthbar.setHealth(piperHealth);

        if (piperHealth <= 0 && isAlive)
        {
            isAlive = false;
            gameManager.gameOver();
            Debug.Log("Dead");
        }


        // Piper's projectile interactions
        secondsBetweenPaperBallSpawn += Time.deltaTime;
        if ((PaperBallScript.activePaperBalls < maxActivePaperBalls) && (secondsBetweenPaperBallSpawn > 1))
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
    void auntyInflictDamage()
    {
        piperHealth -= 10;
    }
    void cleanerInflictDamage()
    {
        piperHealth -= 20;
    }
    void handbagInflictDamage()
    {
        piperHealth -= 10;
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
    void spawnAunty()
    {
        ObjectPoolScript.spawnObject(aunty, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void spawnCleaner()
    {
        ObjectPoolScript.spawnObject(cleaner, SpawnScript.generateSpawnPoint(), Quaternion.identity);
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
        CancelInvoke("spawnAunty");
        CancelInvoke("spawnCleaner");
        BollardScript.bollardCollisionEvent -= bollardInflictDamage;
        FreshieScript.freshieCollisionEvent -= freshieInflictDamage;
        AuntyScript.auntyCollisionEvent -= auntyInflictDamage;
        HandbagScript.handbagCollisionEvent -= handbagInflictDamage;
        CleanerScript.cleanerCollisionEvent -= cleanerInflictDamage;
    }
}
