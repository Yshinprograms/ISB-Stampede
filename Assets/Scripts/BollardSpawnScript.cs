using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Static class to allow for access from LogicScript
public class BollardSpawnScript : MonoBehaviour
{
    public GameObject bollard;
    public float secondsBetweenSpawn = 6f;
    //Spawn Bollards along perimeter of map, outside of camera view
    private float yBound = 5.5f;
    private float xBound = 9.5f;

    // Start is called before the first frame update
    void Start()
    {
        bollard = GameObject.FindGameObjectWithTag("Bollard");
        InvokeRepeating("spawnBollard", 0f, secondsBetweenSpawn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnBollard()
    {
        //Pick a random side and point to spawn from
        int side = Random.Range(1, 5); //4 sides, Max exclusive
        Vector2 spawnPoint = generateSpawnPoint(side);
        Debug.Log(side);
        //Spawn Bollard
        Instantiate(bollard, spawnPoint, Quaternion.identity);
    }

    //Helper function to generate a random spawnpoint on the given side
    Vector2 generateSpawnPoint(int side)
    {
        Vector2 spawnPoint = new Vector2(0, 0);
        switch (side)
        {
            case 1: // Top
                spawnPoint = new(Random.Range(-xBound, xBound), yBound);
                break;
            case 2: // Right
                spawnPoint = new(xBound, Random.Range(-yBound, yBound));
                break;
            case 3: // Bottom
                spawnPoint = new(Random.Range(-xBound, xBound), -yBound);
                break;
            case 4: // Left
                spawnPoint = new(-xBound, Random.Range(-yBound, yBound));
                break;
        }

        return spawnPoint;
    }
}