using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBBullet2 : MonoBehaviour
{

    public delegate void SBBullet2Event();
    public static event SBBullet2Event sbBullet2CollisionEvent;

    private float bulletSpeed;
    private float timeActive;

    //private Vector2 target1 = new(12, -1f);
    private Vector2 target2 = new(12, 7f);

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
        transform.position = Vector2.MoveTowards(transform.position, target2, bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (sbBullet2CollisionEvent != null)
            {
                sbBullet2CollisionEvent();
            }
            // Reset all variables to start values
            timeActive = 0;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }
}
