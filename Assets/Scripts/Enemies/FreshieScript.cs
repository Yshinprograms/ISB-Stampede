using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Freshie given 10 health to start with
public class FreshieScript : MonoBehaviour
{
    public delegate void FreshieCollision();
    public static event FreshieCollision freshieCollisionEvent;
    
    // Movement speed of freshie enemy
    public float freshieSpeed = 1f;

    public float zigzagFrequency = 1f;   // Frequency of the zigzag (how fast it zigzags)
    public float zigzagAmplitude = 1f;   // Amplitude of the zigzag (how wide it zigzags)


    // Health of Freshie
    public int maxHealth = 20;
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
        
        // Calculate the perpendicular direction for the zigzag effect
        Vector3 perpendicularDirection = Vector3.Cross(directionToPiper, Vector3.one);

        // Create a zigzag effect using a sine wave
        float zigzag = Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;

        // Calculate the new position with the zigzag effect
        Vector3 movement = directionToPiper * freshieSpeed * Time.deltaTime;
        Vector3 zigzagMovement = perpendicularDirection * zigzag * Time.deltaTime;

        // Move the character towards Piper with the zigzag effect applied
        transform.position += movement + zigzagMovement;


        // Destroy Freshie when health <= 0
        if (health <= 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
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
