using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aunties given 20 health to start with

public class AuntyScript : MonoBehaviour
{
    public GameObject handbag;
    
    public delegate void AuntyEvent();
    public static event AuntyEvent auntyCollisionEvent;
    public static event AuntyEvent auntyThrowEvent;
    public float auntySpeed;
    public int maxHealth = 20;
    public int health;

    private float distToPiper;
    private float throwRange = 3f;
    private Vector3 directionToPiper;
    private bool hasThrown;

    void Start()
    {
        health = maxHealth;
        auntySpeed = 0.7f;
        hasThrown = false;
    }

    void OnEnable()
    {
        health = maxHealth;
        auntySpeed = 0.7f;
        hasThrown = false;
}
    void Update()
    {
        // Find distance between Aunty and Piper
        distToPiper = Vector3.Distance(transform.position, PiperScript.piperPosition);

        // Direction of Piper from Aunty
        directionToPiper = PiperScript.piperPosition - transform.position;

        // Normalize for constant speed in all directions
        moveToPiper(directionToPiper.normalized);

        // Trigger throw sequence when distance <= 3
        if (distToPiper - throwRange <= 0 && !hasThrown)
        {
            hasThrown = true;
            StartCoroutine(auntyThrowSequence());
        }

        // Return Aunty to objectPool when health <= 0
        if (health <= 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }

    private IEnumerator auntyThrowSequence()
    {
        float throwDelay = 1f;
        float moveDelay = 1f;

        // 1. Stop moving
        auntySpeed = 0f;

        // 2. Spawn Handbag to the right of the Aunty, both Aunty handbagScript waits for 1s before locking on and throwing
        ObjectPoolScript.spawnObject(handbag, transform.position + Vector3.right, Quaternion.identity);
        auntyThrowEvent();
        hasThrown = true;
        yield return new WaitForSeconds(throwDelay);

        // 3. After handbag thrown, wait 1s before resuming movement
        yield return new WaitForSeconds(moveDelay);

        // 4. Resume movement
        hasThrown = false;
        auntySpeed = 0.7f;
    }

    void moveToPiper(Vector3 directionToPiper)
    {
        transform.position += directionToPiper * auntySpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (auntyCollisionEvent != null)
            {
                auntyCollisionEvent();
            }
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }
    }
}