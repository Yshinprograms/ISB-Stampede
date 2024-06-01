using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CSMugger starts with 20 Health

public class CSMuggerScript : MonoBehaviour
{
    public delegate void CSMuggerCollision();
    public static event CSMuggerCollision csMuggerCollisionEvent;
    public float csMuggerSpeed = 0.7f;
    public int maxHealth = 20;
    public int health;

    private float timeBtwCode;
    public float startTimeBtwCode = 10;
    private float codeCount = 0;

    public GameObject csMuggerProjectile;
    void Start()
    {
        health = maxHealth;
        timeBtwCode = startTimeBtwCode;
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

        // Set up code frequency
        if (codeCount < 3)
        {
            if (timeBtwCode <= 0)
            {
                ObjectPoolScript.spawnObject(csMuggerProjectile, transform.position, Quaternion.identity);
                timeBtwCode = startTimeBtwCode;
                codeCount += 1;
            }
            else
            {
                timeBtwCode -= Time.deltaTime;
            }
        }

        // Destroy CS Mugger when health <= 0
        if (health <= 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

        Debug.DrawLine(transform.position, PiperScript.piperPosition, Color.red);
    }

    void moveToPiper(Vector3 directionToPiper)
    {
        transform.position += directionToPiper * csMuggerSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (csMuggerCollisionEvent != null)
            {
                csMuggerCollisionEvent();
            }
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 15;
        }
    }
}
