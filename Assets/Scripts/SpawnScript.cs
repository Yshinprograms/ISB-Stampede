using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnScript
{
    // Map parameters
    private static readonly float yBound = 5.5f;
    private static readonly float xBound = 9.5f;
    private static readonly float yInternalBound = 4.7f;
    private static readonly float xInternalBound = 8.6f;

    // Spawn function to spawn enemy randomly around map perimeter
    public static Vector2 generateSpawnPoint()
    {
        // Pick a random side, 1-4 corresponds to Up, Down, Left, Right
        int side = Random.Range(1, 5); // Max exclusive

        Vector2 spawnPoint = Vector2.zero; // Initialize to zero vector

        switch (side)
        {
            case 1: // Top
                spawnPoint = new Vector2(Random.Range(-xBound, xBound), yBound);
                break;
            case 2: // Right
                spawnPoint = new Vector2(xBound, Random.Range(-yBound, yBound));
                break;
            case 3: // Bottom
                spawnPoint = new Vector2(Random.Range(-xBound, xBound), -yBound);
                break;
            case 4: // Left
                spawnPoint = new Vector2(-xBound, Random.Range(-yBound, yBound));
                break;
            default:
                Debug.LogError("Invalid side value in generateSpawnPoint()");
                break;
        }

        return spawnPoint;
    }

    public static Vector2 generateMapPosition()
    {
        Vector2 mapPosition = Vector2.zero;
        mapPosition.x = Random.Range(-xInternalBound, xInternalBound);
        mapPosition.y = Random.Range(-yInternalBound, yInternalBound);

        return mapPosition;
    }

}
