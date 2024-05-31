using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CSMugger starts with 30 Health

public class CSMuggerScript : MonoBehaviour
{
    public delegate void CSMuggerCollision();
    public static event CSMuggerCollision csMuggerCollisionEvent;
    public float csMuggerSpeed = 0.7f;
    public int maxHealth = 20;
    public int health;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject csMuggerProjectile;
    void Start()
    {
        health = maxHealth;
        timeBtwShots = startTimeBtwShots;
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

        // Set up projectile frequency
        if (timeBtwShots <= 0)
        {
            Instantiate(csMuggerProjectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        } else
        {
            timeBtwShots -= Time.deltaTime;
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
