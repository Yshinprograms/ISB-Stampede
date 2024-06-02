using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bollards given 10 health to start with

public class Bollard : Enemy
{
    public delegate void BollardCollision();
    public static event BollardCollision bollardCollisionEvent;


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
