using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBBullet4 : MonoBehaviour
{
    public delegate void SBBullet4Event();
    public static event SBBullet4Event sbBullet4CollisionEvent;

    private float bulletSpeed;
    private float timeActive;

    //private Vector2 target1 = new(12, -4f);
    //private Vector2 target2 = new(12, 6f);
    //private Vector2 target3 = new(-12, 7f);
    private Vector2 target4 = new(-12, -4f);

    void OnEnable()
    {
        timeActive = 0;
        bulletSpeed = 4f;

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection();

        // Deactivate handbag if no collision after 10s
        if (timeActive > 10)
        {
            timeActive = 0;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
        timeActive += Time.deltaTime;
    }

    void moveDirection()
    {
        transform.position = Vector2.MoveTowards(transform.position, target4, bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (sbBullet4CollisionEvent != null)
            {
                sbBullet4CollisionEvent();
            }
            // Reset all variables to start values
            timeActive = 0;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }
}
