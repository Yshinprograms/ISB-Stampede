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


    void Start()
    {
        // Bollard interaction and Spawns
        BollardScript.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating("spawnBollard", 0f, SpawnScript.secondsBetweenBollardSpawn); // Calls spawnBollard every 6s from t=0

        // Piper's projectile interactions
        InvokeRepeating("spawnPaperBall", 0f, SpawnScript.secondsBetweenPaperBallSpawn); // Calls spawnBollard every 6s from t=0

        // Freshie interaction and Spawns + Future enemies
    }


    void Update() => healthbar.text = piperHealth.ToString();

    // Damages
    void bollardInflictDamage()
    {
        piperHealth -= 10;
    }

    // Spawn Enemies
    void spawnBollard()
    {
        Instantiate(bollard, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }

    // Spawn Projectiles
    void spawnPaperBall()
    {
        // Doesn't matter what spawn position, because PaperBallScript Start() position is to the right of Piper
        Instantiate(paperBall, PiperScript.piperPosition, Quaternion.identity);
        paperBall.SetActive(true);
    }

    // Unsubscribe from Events
    private void OnDestroy()
    {
        BollardScript.bollardCollisionEvent -= bollardInflictDamage;
    }
}
