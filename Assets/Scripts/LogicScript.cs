using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Handles all UI and logic in game

public class LogicScript : MonoBehaviour
{
    public int piperHealth = 100;
    public Text healthbar;

    // Import Game Manager Script & Game end conditions
    public GameManagerScript gameManager;
    private bool isDead = false;

    // Import GameObjects, drag and drop into Inspector
    public GameObject bollard;
    public GameObject paperBall;
    public GameObject freshie;

    // Spawn times
    public float secondsBetweenPaperBallSpawn = 6f;
    public float secondsBetweenBollardSpawn = 6f;
    public float secondsBetweenFreshieSpawn = 6f;

    void Start()
    {
        // Bollard interaction and Spawns
        BollardScript.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating("spawnBollard", 0f, secondsBetweenBollardSpawn); // Calls spawnBollard every 6s from t=0

        // Piper's projectile interactions
        InvokeRepeating("spawnPaperBall", 0f, secondsBetweenPaperBallSpawn); // Calls spawnBollard every 3s from t=0

        // Freshie interaction and Spawns + Future enemies
        FreshieScript.freshieCollisionEvent += freshieInflictDamage;
        InvokeRepeating("spawnFreshie", 0f, secondsBetweenFreshieSpawn); // Calls freshieBollard every 3s from t=0
    }


    void Update()
    {
        // Set minimum health to 0
<<<<<<< Updated upstream
        piperHealth = Mathf.Clamp(piperHealth, 0, 100);
        healthbar.text = piperHealth.ToString();
=======
        //piperHealth = Mathf.Clamp(piperHealth, 0, 100);
        healthbar.setHealth(piperHealth);
>>>>>>> Stashed changes

        if (piperHealth <= 0 && !isDead)
        {
            isDead = true;
            gameManager.gameOver();
            Debug.Log("Dead");
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
        CancelInvoke("spawnPaperBall");
        CancelInvoke("spawnFreshie");
        BollardScript.bollardCollisionEvent -= bollardInflictDamage;
        FreshieScript.freshieCollisionEvent -= freshieInflictDamage;

    }
}
