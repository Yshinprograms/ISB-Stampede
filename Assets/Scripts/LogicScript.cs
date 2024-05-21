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

    // Import GameObjects, drag and drop into Inspector
    public GameObject bollard;
    public GameObject paperBall;

    // Spawn times
    public float secondsBetweenPaperBallSpawn = 6f;
    public float secondsBetweenBollardSpawn = 6f;

    void Start()
    {
        // Bollard interaction and Spawns
        BollardScript.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating("spawnBollard", 0f, secondsBetweenBollardSpawn); // Calls spawnBollard every 6s from t=0

        // Piper's projectile interactions
        InvokeRepeating("spawnPaperBall", 0f, secondsBetweenPaperBallSpawn); // Calls spawnBollard every 3s from t=0

        // Freshie interaction and Spawns + Future enemies
    }


    void Update()
    {
        healthbar.text = piperHealth.ToString();
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
        CancelInvoke("spawnPaperBall");
        BollardScript.bollardCollisionEvent -= bollardInflictDamage;
    }
}
