using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class ObjectPoolScript : MonoBehaviour
{
    public static List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();

    public static GameObject spawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        // Create a local pool of PooledObjectInfo class
        PooledObjectInfo pool = null;

        // Check if there is a matching pool that already exists within objectPools list
        foreach (PooledObjectInfo p in objectPools)
        {
            if (p.LookupString == objectToSpawn.name)
            {
                // Set the local pool to the respective pool within list if found and exit loop
                pool = p;
                break;
            }
        }

        // If respective pool not found within our list, create the pool and add it to our list
        if (pool == null)
        {
            // Change the name of our newly created pool to the name of the object we want to spawn
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            objectPools.Add(pool);
        }

        // Now within our found or newly created pool, reactivate any inactive objects or instantiate new ones
        // Checks if there are any inactive objects using the system.linq namespace
        GameObject spawnObject = pool.inactiveObjects.FirstOrDefault();
        if (spawnObject == null)
        {
            // If no inactive objects, instantiate a new one
            spawnObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        }
        else
        {
            // Just need to reinitialise the inactive object to where it's suppose to be if it spawns
            // since FirstorDefault() already found us an inactive object
            spawnObject.transform.position = spawnPosition;
            spawnObject.transform.rotation = spawnRotation;

            // Remove this object from the list of inactive objects
            pool.inactiveObjects.Remove(spawnObject);
            spawnObject.SetActive(true);
        }

        return spawnObject;
    }

    public static void returnObjectToPool(GameObject obj)
    {
        // We need to remove the last 7 char because it's (clone)
        string name = obj.name.Substring(0, obj.name.Length - 7);
        // Look for the pool that belongs to the object
        PooledObjectInfo pool = objectPools.Find(p => p.LookupString == name);

        if (pool == null)
        {
            Debug.LogWarning("Trying to release an object that is not pooled: " + name);
        }
        else
        {
            // Return object back into pool
            obj.SetActive(false);
            pool.inactiveObjects.Add(obj);
        }
    }
}

public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}

