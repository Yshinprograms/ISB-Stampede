using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManagerScript : MonoBehaviour
{
    // Singleton Implementation
    private static PowerUpManagerScript instance;
    public static PowerUpManagerScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PowerUpManagerScript>();
                if (instance == null)
                {
                    GameObject go = new("PowerUpManager");
                    instance = go.AddComponent<PowerUpManagerScript>();
                }
            }
            return instance;
        }
    }

    public GameObject healthPowerUp;
    public GameObject mala;
    public GameObject friend;

    public float secondsBetweenHealthPowerUpSpawnMin = 50f;
    public float secondsBetweenHealthPowerUpSpawnMax = 90f;
    public float timeBetweenMalaSpawn = 15f;
    public float timeBetweenFriendSpawn = 20f;
    public bool levelTwo;
    public bool levelThree;

    private void Start()
    {
        Invoke(nameof(SpawnHealthPowerUpRandom), Random.Range(secondsBetweenHealthPowerUpSpawnMin, secondsBetweenHealthPowerUpSpawnMax));
        Invoke(nameof(SpawnFriends), timeBetweenFriendSpawn);
        Invoke(nameof(SpawnMala), timeBetweenMalaSpawn);
    }

    public void RestartScript()
    {
        levelTwo = false;
        levelThree = false;
    }

    public void SpawnHealthPowerUp()
    {
        ObjectPoolScript.spawnObject(healthPowerUp, SpawnScript.generateMapPosition(), healthPowerUp.transform.rotation);
    }
    public void SpawnHealthPowerUpRandom()
    {
        if (Random.value < 0.5f)
        {
            SpawnHealthPowerUp();
        }
        // Schedule the next spawn attempt with a random delay throughout the entire game
        Invoke(nameof(SpawnHealthPowerUpRandom), Random.Range(secondsBetweenHealthPowerUpSpawnMin, secondsBetweenHealthPowerUpSpawnMax));
    }
    public void SpawnFriends()
    {
        Debug.Log("Spawn Friends");

        //levelTwo
        if (true)
        {
            ObjectPoolScript.spawnObject(friend, SpawnScript.generateSpawnPoint(), friend.transform.rotation);
        }
        Invoke(nameof(SpawnFriends), timeBetweenFriendSpawn);
    }
    public void SpawnMala()
    {
        //levelThree
        if (true)
        {
            PiperScript.malaActive = true;
            ObjectPoolScript.spawnObject(mala, PiperScript.piperPosition + (4 * Vector3.up), mala.transform.rotation);
        }
        Invoke(nameof(SpawnMala), timeBetweenMalaSpawn);
    }

}
