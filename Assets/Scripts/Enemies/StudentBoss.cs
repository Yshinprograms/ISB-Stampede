using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

/* Boss will move from Point A to B to C to D, then
 * Boss will stop for 5 seconds and shoot objects hexagonally
 * Boss will then move from Point D to E to F to G
 * Boss will move from Point G to F to E to D
 * Boss will stop for 5 seconds and shoot objects hexagonally
 * Boss will move from Point D to C to B to A 
 * This will repeat 
*/

public class StudentBoss : Enemy
{
    public BossHealthbarScript bossHealthbarScript;

    // Points
    public GameObject p0;
    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject p5;
    public GameObject p6;
    public GameObject p7;
    public GameObject p8;
    public GameObject p9;

    private Transform currentPoint;
    public bool isAtStartPosition;
    public float tolerance = 0.01f;


    private void OnEnable()
    {      
        bossHealthbarScript.EnableHealthbar();
        bossHealthbarScript.SetMaxHealth((int)maxHealth);
        health = maxHealth;
        isAtStartPosition = false;

    }

    void Update()
    {
        bossHealthbarScript.SetHealth((int)health);
        
        // Boss goes to start position
        if (!isAtStartPosition)
        {
            // Debug.Log("this is okay");
            GoToStartPosition();
        }

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = p0.transform.position;

        if (Vector2.Distance(currentPosition, targetPosition) < tolerance)
        {
            isAtStartPosition = true;
        }

        if (isAtStartPosition)
        {
            //transform.position = Vector2.MoveTowards(transform.position, p1.transform.position, moveSpeed * Time.deltaTime);
            //Moving();
            StartCoroutine(BossMovementRoutine());
        }
        //Move();

        if (health <= 0)
        {
            bossHealthbarScript.DisableHealthbar();
            gameObject.SetActive(false);
            L1LogicScript.Instance.bossBattle = false;
            L1LogicScript.Instance.LevelCompleted();
        }
    }

    void GoToStartPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, p0.transform.position, moveSpeed * Time.deltaTime);
        if (transform.position == p0.transform.position)
        {
            Debug.Log("startpos is true");
            isAtStartPosition = true;
        }
    }

    /*void MoveToNextPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
    }*/

    IEnumerator BossMovementRoutine()
    {
        currentPoint = p1.transform;
        // Move to Point B
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
        //MoveToNextPoint();
        if (transform.position ==  p1.transform.position)
        {
            currentPoint = p2.transform;
            transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, moveSpeed * Time.deltaTime);
        }
        yield return null;
    }


    protected override void Move()
    {

    }

    protected override void TurnDirection()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }
    }
}
