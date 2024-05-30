using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bollards given 10 health to start with

public class BollardScript : MonoBehaviour
{
    public delegate void BollardEvent();
    public static event BollardEvent bollardCollisionEvent;
    public float bollardSpeed = 0.7f;
    public int maxHealth = 10;
    public int health;

    void Start()
    {
        health = maxHealth;
    }

    void OnEnable()
    {
        health = maxHealth;
    }
    void Update()
    {
        // Vector Addition
        Vector3 directionToPiper = PiperScript.piperPosition - transform.position;

        // Normalize for constant speed in all directions
        moveToPiper(directionToPiper.normalized); 

        // Destroy Bollard when health <= 0
        if (health <= 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }

    void moveToPiper(Vector3 directionToPiper)
    {
        transform.position += directionToPiper * bollardSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (bollardCollisionEvent != null)
            {
                bollardCollisionEvent();
            }
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }
    }
}
