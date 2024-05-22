using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBallScript : MonoBehaviour
{
    public static int activePaperBalls;
    public delegate void PaperBallEvent();
    public static event PaperBallEvent paperBallCollisionEvent;
    public static event PaperBallEvent paperBallThrownEvent;
    public static bool paperBallThrown;
    public float paperBallSpeed = 4f;

    private GameObject targetEnemy;

    void Start()
    {
        targetEnemy = null;
        paperBallThrown = false;
    }

    void Update()
    {
        // If enemy in range, lock onto targetEnemy and get its location
        if (PiperScript.enemyInRange != null && targetEnemy == null)
        {
            findClosestEnemy();
            // Play throwing audio
            if (!paperBallThrown)
            {
                paperBallThrownEvent();
                paperBallThrown = true;
            }
        }

        // Move to enemy even if no longer within range
        if (targetEnemy != null)
        {
            Vector3 directionToEnemy = targetEnemy.transform.position - gameObject.transform.position; // Vector Addition
            moveToEnemy(directionToEnemy.normalized); // Normalize for constant speed in all directions
        }
        // Otherwise stay beside Piper & keep finding
        else
        {
            transform.position = PiperScript.piperPosition + Vector3.right;
        }
    }

    void moveToEnemy(Vector3 directionToEnemy)
    {
        transform.position += directionToEnemy * paperBallSpeed * Time.deltaTime;
    }

    void findClosestEnemy()
    {
        {
            // Find all enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            // Initialize variables for the closest enemy
            GameObject closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            // Loop through all enemies to find the closest one
            foreach (GameObject enemy in enemies)
            {
                // Calculate the distance to the current enemy
                float distance = Vector2.Distance(PiperScript.piperPosition, enemy.transform.position);

                // If this enemy is closer than the current closest enemy, update the closest enemy
                if (distance < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }

            // Set the target enemy
            targetEnemy = closestEnemy;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            paperBallCollisionEvent();

            // Destroy after a single collision and clear targetEnemy
            ObjectPoolScript.returnObjectToPool(gameObject);

            // Clear targetEnemy lockon when we reactivate from object pool
            targetEnemy = null;

            // Reset paperBall to not thrown when it reactivates
            PaperBallScript.paperBallThrown = false;

            // Reset paperBall count to zero for next throw on reactivation
            activePaperBalls -= 1;
        }
    }
}
