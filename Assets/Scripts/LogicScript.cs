using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int piperHealth = 100;
    public Text healthbar;

    // Bollard parameters
    public static GameObject bollard;
    public static float secondsBetweenBollardSpawn = 6f;


    void Start()
    {
        // Bollard interaction and Spawns
        BollardScript.bollardCollision += bollardInflictDamage;
        bollard = GameObject.FindGameObjectWithTag("Bollard");
        InvokeRepeating("spawnBollard", 0f, secondsBetweenBollardSpawn); // Calls spawnBollard every 6s from t=0

        // Freshie interaction and Spawns + Future enemies
    }


    void Update() => healthbar.text = piperHealth.ToString();

    void bollardInflictDamage()
    {
        piperHealth -= 10;
    }

    void spawnBollard()
    {
        Instantiate(bollard, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }

    private void OnDestroy()
    {
        BollardScript.bollardCollision -= bollardInflictDamage;
    }
}
