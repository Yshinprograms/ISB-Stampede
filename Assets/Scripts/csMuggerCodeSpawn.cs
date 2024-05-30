using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMuggerCodeSpawnScript : MonoBehaviour
{
    public delegate void CSMuggerCodeCollision();
    public static event CSMuggerCodeCollision csMuggerCodeCollisionEvent;
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
        // Destroy CS Mugger Projectile when health <= 0
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (csMuggerCodeCollisionEvent != null)
            {
                csMuggerCodeCollisionEvent();
            }
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }
    }
}
