using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS1010ProjectileScript : MonoBehaviour
{
    public float speed;
    public delegate void CS1010ProjectileEvent();
    public static event CS1010ProjectileEvent CS1010ProjectileCollisionEvent;

    private float timeActive;
    private float delayTimeBetweenSpawnAndThrown;
    private Vector3 directionToPiper;

    private void OnEnable()
    {
        speed = 4f;
        timeActive = 0;
        delayTimeBetweenSpawnAndThrown = 0;
        directionToPiper = Vector3.zero;
    }

    void Update()
    {
        // Get directionToPiper only once after 1s
        // if (delayTimeBetweenSpawnAndThrown > 1 && directionToPiper == Vector3.zero)
        if (directionToPiper == Vector3.zero)
        {
            directionToPiper = PiperScript.piperPosition - transform.position;
        }

        transform.position += speed * Time.deltaTime * directionToPiper.normalized;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CS1010ProjectileCollisionEvent?.Invoke();
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }
}
