using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// USE OBJECT POOLING

public class PaperBallScript : MonoBehaviour
{
    public delegate void PaperBallCollision();
    public static event PaperBallCollision paperBallCollisionEvent;
    public float paperBallSpeed = 2f;

    private GameObject targetEnemy;

    void Start()
    {
        transform.position = PiperScript.piperPosition + Vector3.right;
        findClosestEnemy();
    }

    void Update()
    {
        if (targetEnemy != null)
        {
            Vector3 directionToEnemy = targetEnemy.transform.position - gameObject.transform.position; // Vector Addition
            moveToEnemy(directionToEnemy.normalized); // Normalize for constant speed in all directions
        }
        else
        {
            findClosestEnemy();
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

            // Destroy after a single collision
            Destroy(gameObject);
        }
    }
}
