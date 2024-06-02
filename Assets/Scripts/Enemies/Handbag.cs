using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handbag : MonoBehaviour
{
    public float handbagSpeed;
    public delegate void HandBagEvent();
    public static event HandBagEvent handbagCollisionEvent;

    private float timeActive;
    private float delayTimeBetweenSpawnAndThrown;
    private Vector3 directionToPiper;

    // Start is called before the first frame update
    void Start()
    {
        handbagSpeed = 4f;
        timeActive = 0;
        delayTimeBetweenSpawnAndThrown = 0;
        directionToPiper = Vector3.zero;
    }

    void Update()
    {
        // Get directionToPiper only once after 1s
        if (delayTimeBetweenSpawnAndThrown > 1 && directionToPiper == Vector3.zero)
        {
            directionToPiper = PiperScript.piperPosition - transform.position;
        }

        // Normalize for constant speed in all directions
        moveToPiper(directionToPiper.normalized);

        // Deactivate handbag if no collision after 10s
        if (timeActive > 10)
        {
            timeActive = 0;
            delayTimeBetweenSpawnAndThrown = 0;
            directionToPiper = Vector3.zero;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

        timeActive += Time.deltaTime;
        delayTimeBetweenSpawnAndThrown += Time.deltaTime;
    }

    void moveToPiper(Vector3 directionToPiper)
    {
        transform.position += directionToPiper * handbagSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (handbagCollisionEvent != null)
            {
                handbagCollisionEvent();
            }
            // Reset all variables to start values
            timeActive = 0;
            delayTimeBetweenSpawnAndThrown = 0;
            directionToPiper = Vector3.zero;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }
}
