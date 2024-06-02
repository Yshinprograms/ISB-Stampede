using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnScript
{
    // Position outside the map, so +0.5f as margin
    private static readonly float xBound = 9.5f;
    private static readonly float yUpperBound = 10.7f;
    private static readonly float yLowerBound = -6f;
    /*private static readonly float xBound = 8f;
    private static readonly float yUpperBound = 10f;
    private static readonly float yLowerBound = -1f;*/

    // Position within the map
    private static readonly float xInternalBound = 8.4f;
    private static readonly float yInternalUpperBound = 9.4f;
    private static readonly float yInternalLowerBound = -4.4f;

    // Position at corners of the map
    private static readonly float xEnginGatheringCorner = 8f;
    private static readonly float yUpperEnginGatheringCorner = 9f;
    private static readonly float yLowerEnginGatheringCorner = -4f;

    // Spawn function to spawn enemy randomly around map perimeter
    public static Vector2 generateSpawnPoint()
    {
        // Pick a random side, 1-4 corresponds to Up, Down, Left, Right
        int side = Random.Range(1, 5); // Max exclusive

        Vector2 spawnPoint = Vector2.zero; // Initialize to zero vector

        switch (side)
        {
            case 1: // Top
                spawnPoint = new Vector2(Random.Range(-xBound, xBound), yUpperBound);
                break;
            case 2: // Right
                spawnPoint = new Vector2(xBound, Random.Range(yLowerBound, yUpperBound));
                break;
            case 3: // Bottom
                spawnPoint = new Vector2(Random.Range(-xBound, xBound), yLowerBound);
                break;
            case 4: // Left
                spawnPoint = new Vector2(-xBound, Random.Range(yLowerBound, yUpperBound));
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
        mapPosition.y = Random.Range(yInternalLowerBound, yInternalUpperBound);

        return mapPosition;
    }

    public static Vector2 generateEnginKidGatheringCorner()
    {
        Vector2 mapPosition = Vector2.zero;
        int corner = Random.Range(1, 5);
        switch (corner)
        {
            case 1: // Top
                mapPosition = new Vector3(xEnginGatheringCorner, yUpperEnginGatheringCorner);
                break;
            case 2: // Right
                mapPosition = new Vector3(xEnginGatheringCorner, yLowerEnginGatheringCorner);
                break;
            case 3: // Bottom
                mapPosition = new Vector3(-xEnginGatheringCorner, yUpperEnginGatheringCorner);
                break;
            case 4: // Left
                mapPosition = new Vector3(-xEnginGatheringCorner, yLowerEnginGatheringCorner);
                break;
            default:
                Debug.LogError("Invalid side value in generateSpawnPoint()");
                break;
        }

        return mapPosition;
    }

}
