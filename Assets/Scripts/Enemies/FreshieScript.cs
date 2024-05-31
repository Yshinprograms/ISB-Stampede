using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Freshie given 20 health to start with
public class FreshieScript : MonoBehaviour
{
    public delegate void FreshieCollision();
    public static event FreshieCollision freshieCollisionEvent;
    
    // Movement speed of freshie enemy
    public float freshieLinearSpeed = 1.5f;
    public float freshieDiagonalSpeed = 1f;
    public float moveDuration = 1f;  
    public float stopDuration = 1f;
    private bool isMoving = true;

    // Health of Freshie
    public int maxHealth = 20;
    public int health;

    // Start is called before the first frame update
    private void OnEnable()
    {
        health = maxHealth;
        StartCoroutine(FreshieZigzagMovement());
    }

    void Start()
    {
        health = maxHealth;
        //StartCoroutine(FreshieZigzagMovement());

    }

    // Update is called once per frame
    void Update()
    {
        // Destroy Freshie when health <= 0
        if (health <= 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }


    // coroutine of freshie
    IEnumerator FreshieZigzagMovement()
    {
        while (true)
        {
            // Move towards player horizontally for moveDuration
            yield return MoveTowardsPiper(Vector2.right * (PiperScript.piperPosition.x <= transform.position.x ? -1 : 1), moveDuration, freshieLinearSpeed);

            // Stop for stopDuration
            isMoving = false;
            yield return new WaitForSeconds(stopDuration);
            isMoving = true;

            // Move diagonally for moveDuration
            Vector2 diagonalDirection = new Vector2(
                PiperScript.piperPosition.x <= transform.position.x ? 1 : -1,
                PiperScript.piperPosition.y <= transform.position.y ? -1 : 1
            );

            yield return MoveTowardsPiper(diagonalDirection, moveDuration, freshieDiagonalSpeed);

            // Stop for stopDuration
            isMoving = false;
            yield return new WaitForSeconds(stopDuration);
            isMoving = true;
        }
    }

    IEnumerator MoveTowardsPiper(Vector2 direction, float duration, float speed)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (isMoving)
            {
                transform.Translate(direction * speed * Time.deltaTime);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
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
