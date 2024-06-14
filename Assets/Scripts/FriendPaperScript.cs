using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendPaperScript : MonoBehaviour
{
    private GameObject targetEnemy;
    private float speed = 5f;
    private bool enemyFound;

    // Start is called before the first frame update
    private void OnEnable()
    {
        targetEnemy = null;
        enemyFound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemyFound)
        {
            FindClosestEnemy();
        }

        if (targetEnemy != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetEnemy.transform.position, Time.deltaTime * speed);
        }

        if (targetEnemy != null && !targetEnemy.activeSelf)
        {
            enemyFound = false;
        }
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
            enemyFound = true;
        }
    }
    //PiperScript.allEnemyMask
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 11)
        {
            enemyFound = false;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }
}
