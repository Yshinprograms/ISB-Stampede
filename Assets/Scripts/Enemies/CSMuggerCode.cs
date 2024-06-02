using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSMuggerCode : Enemy
{
    public delegate void CSMuggerCodeCollision();
    public static event CSMuggerCodeCollision csMuggerCodeCollisionEvent;


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
