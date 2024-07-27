using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBallScript : MonoBehaviour
{
    public static int activePaperBalls = 0;
    public delegate void PaperBallEvent();
    public static event PaperBallEvent paperBallCollisionEvent;
    public static event PaperBallEvent paperBallThrownEvent;
    public static bool paperBallThrown;
    public float paperBallSpeed;

    private GameObject targetEnemy;
    private bool targetWithinRange;
    private bool targetAlive;

    private void OnEnable()
    {
        targetEnemy = null;
        paperBallThrown = false;
        targetWithinRange = false;
        targetAlive = false;
        paperBallSpeed = 5f;
    }
    /*
     * 1. Spawn
     * 2. Wait for enemy to enter range
     *   a. If not entered range,stay beside Piper
     *   b. If entered range:
     *     i. Acquire reference to the nearest target only once
     *     ii. Move towards this target
     *       1. If this target collides with paperball, destroy paperball
     *       2. If target is destroyed before paperball collides, destroy paperball as well
     */
    void Update()
    {
        // 2. Check if enemies are within range in PiperScript
        CheckIfAnyTargetWithinRange();

        // 2.a If enemy in range, lock onto targetEnemy and get its location only once
        if (targetWithinRange && !paperBallThrown)
        {
            FindClosestEnemy();
            // Play throwing audio
            PaperBallAudio();
            paperBallThrown = true;
        }

        if (targetEnemy != null)
        {
            // Check if the target is still alive
            CheckIfTargetAlive();
        }

        if (paperBallThrown)
        {
            // 2.b.ii Move to enemy if locked on, else destroyed
            if (targetAlive)
            {
                Debug.DrawLine(transform.position, targetEnemy.transform.position, Color.cyan);
                MoveToEnemy();
            }
            // 2.b.ii.2
            else
            {
                Debug.Log("destroyed");
                ObjectPoolScript.returnObjectToPool(gameObject);
                activePaperBalls -= 1;
            }
        }
        // Stay beside Piper & keep finding if not launched
        else
        {
            transform.position = PiperScript.piperRealPosition + Vector3.right;
        }
    }

    void CheckIfAnyTargetWithinRange()
    {
        if (PiperScript.enemyInRange != null)
        {
            targetWithinRange = true;
        }
        else
        {
            targetWithinRange = false;
        }
    }
    void CheckIfTargetAlive()
    {
        targetAlive = targetEnemy.activeSelf;
    }

    void PaperBallAudio()
    {
        if (!paperBallThrown)
        {
            paperBallThrownEvent();
        }
    }

    void MoveToEnemy()
    {
        Vector3 directionToEnemy = targetEnemy.transform.position - gameObject.transform.position;
        transform.position += paperBallSpeed * Time.deltaTime * directionToEnemy.normalized;
    }

    void FindClosestEnemy()
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
    //PiperScript.allEnemyMask
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 11)
        {
            paperBallCollisionEvent();

            // Destroy after a single collision and clear targetEnemy
            ObjectPoolScript.returnObjectToPool(gameObject);

            // Reset paperBall count to zero for next throw on reactivation
            activePaperBalls -= 1;
        }
    }
}
