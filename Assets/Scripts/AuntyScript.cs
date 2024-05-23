using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Aunties given 20 health to start with

public class AuntyScript : MonoBehaviour
{
    public GameObject handbag;
    public float handbagSpeed;
    public delegate void AuntyEvent();
    public static event AuntyEvent auntyCollisionEvent;
    public float auntySpeed = 0.7f;
    public int maxHealth = 20;
    public int health;

    private float distToPiper;
    private float throwRange = 3f;
    private Coroutine auntyThrowCoroutine;

    void Start()
    {
        health = maxHealth;
        handbagSpeed = 4f;
    }

    void OnEnable()
    {
        health = maxHealth;
    }
    void Update()
    {
        // Find distance between Aunty and Piper
        distToPiper = Vector2.Distance(transform.position, PiperScript.piperPosition);

        // Vector Addition to find direction of Piper from Aunty
        Vector3 directionToPiper = PiperScript.piperPosition - transform.position;

        // Normalize for constant speed in all directions
        moveToPiper(directionToPiper.normalized);

        // Trigger throw sequence when distance <= 3
        if (distToPiper - throwRange <= 0)
        {
            auntyThrowCoroutine = StartCoroutine(auntyThrowSequence());
        }

        // Return Aunty to objectPool when health <= 0
        if (health <= 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }

    /* Aunty Throw Sequence:
     * Stop
     * Spawn Handbag
     * Wait 1s
     * Lockon to piperPosition and set flag to true
     * Fire handbag
     * Wait 1s
     * Exit Coroutine
     */
    private IEnumerator auntyThrowSequence()
    {
        float loadDelay = 1f;
        float moveDelay = 1f;

        // 1. Stop moving
        auntySpeed = 0f;

        // 2. Spawn Handbag 
        ObjectPoolScript.spawnObject(handbag, transform.position, Quaternion.identity);

        // 3. Wait for the specified delay
        yield return new WaitForSeconds(loadDelay);

        // 4. Lock onto Piper's position and fire
        Vector3 direction = (PiperScript.piperPosition - transform.position).normalized;
        handbag.GetComponent<Rigidbody2D>().velocity = direction * handbagSpeed;

        // 5. Wait again before resuming movement
        yield return new WaitForSeconds(moveDelay);

        // 6. Resume movement
        auntySpeed = 0.7f;
    }

    void moveToPiper(Vector3 directionToPiper)
    {
        transform.position += directionToPiper * auntySpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
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