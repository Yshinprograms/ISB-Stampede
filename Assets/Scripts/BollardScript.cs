using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fix Bollard Parent starting position?
// Bollards given 10 health to start with

public class BollardScript : MonoBehaviour
{
    public delegate void BollardCollision();
    public static event BollardCollision bollardCollisionEvent;
    public float bollardSpeed = 0.7f;

    //private Transform piperPosition;
    private int bollardHealth = 10;

    void Start()
    {
        //piperPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        Vector3 directionToPiper = PiperScript.piperPosition - transform.position; // Vector Addition
        moveToPiper(directionToPiper.normalized); // Normalize for constant speed in all directions

        // Destroy Bollard when health <= 0
        if (bollardHealth <= 0)
        {
            Destroy(gameObject);
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
            bollardHealth -= 10;
        }
    }
}
