using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
1. Med Student will wander around
2. If Piper is in range, Med Student will attack piper (move towards Piper x2 speed)
3. If Piper is not in range, Med student will shoot poisonous chemical around in random intervals 

Poisonous chemical
- will stay in the map for 5s and disappear 
*/

public class MedStudent : Enemy
{
    public delegate void MedCollision();
    public static event MedCollision medCollisionEvent;

    private float timeBtwSpray;
    private int maxTimeBtwSpray = 5;
    private int minTimeBtwSpray = 15;
    [SerializeField] float followDistance;
    [SerializeField] float range;
    [SerializeField] float maxDistance;
    Vector2 wayPoint;

    public GameObject chemicalShot;

    /*void Start()
    {
        health = maxHealth;
        SetNewDestination();
        timeBtwSpray = GenerateRandomNumber(maxTimeBtwSpray, minTimeBtwSpray);
    }*/

    void OnEnable()
    {
        health = maxHealth;
        SetNewDestination();
        timeBtwSpray = GenerateRandomNumber(maxTimeBtwSpray, minTimeBtwSpray);
    }

    protected override void Move()
    {

        if (Vector2.Distance(transform.position, PiperScript.piperPosition) < followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, PiperScript.piperPosition, moveSpeed*1.5f * Time.deltaTime);
        } else
        {
            // med student wanders around 
            transform.position = Vector2.MoveTowards(transform.position, wayPoint, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, wayPoint) < range)
            {
                SetNewDestination();
            }

            // med student shoot chemicals from syringe
            if (timeBtwSpray <= 0)
            {
                // shoot at a random position 
                ObjectPoolScript.spawnObject(chemicalShot, transform.position + Vector3.right, Quaternion.identity);
                // Instantiate(chemicalShot, transform.position + Vector3.right, Quaternion.identity);
                timeBtwSpray = GenerateRandomNumber(maxTimeBtwSpray, minTimeBtwSpray);
            }
            else
            {
                timeBtwSpray -= Time.deltaTime;
            }

        }

    }

    void SetNewDestination()
    {
        wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }

    int GenerateRandomNumber(int min, int max)
    {
        return Random.Range(min, max+1);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (medCollisionEvent != null)
            {
                medCollisionEvent();
            }
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }
    }
}
