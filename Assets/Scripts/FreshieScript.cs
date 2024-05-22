using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Freshie given 10 health to start with
public class FreshieScript : MonoBehaviour
{
    public delegate void FreshieCollision();
    public static event FreshieCollision freshieCollisionEvent;
    public float freshieSpeed = 0.7f;
    public int maxHealth = 10;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector Addition
        Vector3 directionToPiper = PiperScript.piperPosition - transform.position;

        // Normalize for constant speed in all directions
        moveToPiper(directionToPiper.normalized);

        // Destroy Freshie when health <= 0
        if (health <= 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

        Debug.DrawLine(transform.position, PiperScript.piperPosition, Color.red);
    }

    void moveToPiper(Vector3 directionToPiper)
    {
        transform.position += directionToPiper * freshieSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (freshieCollisionEvent != null)
            {
                freshieCollisionEvent();
            }
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }
    }

}
