using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

/* Boss will move from Point 0 to 1 to 2 to 3 to 4, then
 * Boss will stop for 5 seconds and shoot objects hexagonally
 * Boss will then move from Point 5 to 6 to 7 to 8
 * Boss will move from Point 8 to 7 to 6 to 5 to 4 to 3 to 2 to 1 to 0
 * This will repeat 
*/

public class StudentBoss : Enemy
{
    public BossHealthbarScript bossHealthbarScript;
    public delegate void SBEvent();
    public static event SBEvent SBCollisionEvent;

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
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;
    public GameObject bullet5;
    public GameObject bullet6;

    private bool isAtStartPosition;
    private float tolerance = 0.1f;

    public bool bossHasAttacked;
    public bool hasReversed;
    private bool isAtP0;
    private bool isAtP1;
    private bool isAtP2;
    private bool isAtP3;
    private bool isAtP4;
    private bool isAtP5;
    private bool isAtP6;
    private bool isAtP7;
    private bool isAtP8;

    private float timeBtwShot;
    private float levelTimeBtwShot;
    private float L1Speed = 4f;
    private float L1ShootInterval = 1;
    private float L2Speed = 5f;
    private float L2ShootInterval = 0.7f;
    private float L3Speed = 5f;
    private float L3ShootInterval = 0.5f;
    private float L4Speed = 5.5f;
    private float L4ShootInterval = 0.5f;
    private float currentSpeed;



    private void OnEnable()
    {      
        bossHealthbarScript.EnableHealthbar();
        bossHealthbarScript.SetMaxHealth((int)maxHealth);
        health = maxHealth;
        isAtStartPosition = false;
        bossHasAttacked = false;
        hasReversed = false;
        isAtP0 = false;
        isAtP1 = false;
        isAtP2 = false;
        isAtP3 = false;
        isAtP4 = false;
        isAtP5 = false;
        isAtP6 = false;
        isAtP7 = false;
        isAtP8 = false;
        timeBtwShot = L1ShootInterval;
        currentSpeed = L1Speed;

    }

    void Update()
    {
        bossHealthbarScript.SetHealth((int)health);
        UpdateStage();

        // Boss goes to start position
        if (!isAtStartPosition)
        {
            // Debug.Log("this is okay");
            GoToStartPosition();
        }

        if (Vector2.Distance(transform.position, p0.transform.position) < tolerance)
        {
            isAtStartPosition = true;
        }

        if (isAtStartPosition)
        {
            // Move to Point 0
            if (isAtP1 && hasReversed)
            {
                GoToStartPosition();
            }

            if (Vector2.Distance(transform.position, p0.transform.position) < tolerance)
            {
                isAtP0 = true;
                isAtP1 = false;
                hasReversed = false;
                bossHasAttacked = false;
            }

            // Move to Point 1
            if ((isAtP0 && !hasReversed) || (isAtP2 && hasReversed))
            {
                MoveToP1();
                bossHasAttacked = false;
            }
            
            if (Vector2.Distance(transform.position, p1.transform.position) < tolerance)
            {
                isAtP1 = true;
                isAtP2 = false;
                isAtP0 = false;
                bossHasAttacked = false;
            }

            // Move to Point 2
            if ((isAtP1 && !hasReversed) || (isAtP3 && hasReversed))
            {
                MoveToP2();
            }

            if (Vector2.Distance(transform.position, p2.transform.position) < tolerance)
            {
                isAtP2 = true;
                isAtP1 = false;
                isAtP3 = false;
            }

            // Move to Point 3
            if ((isAtP2 && !isAtP1 && !hasReversed) || (isAtP4 && hasReversed))
            {
                moveSpeed = currentSpeed;
                MoveToP3();
            }

            if (Vector2.Distance(transform.position, p3.transform.position) < tolerance)
            {
                isAtP3 = true;
                isAtP2 = false;
                isAtP4 = false;
            }

            // Move to Point 4
            if ((isAtP3 && !isAtP2 && !hasReversed) || (isAtP5 && hasReversed))
            {
                MoveToP4();
            }

            // Boss Attack
            if (Vector2.Distance(transform.position, p4.transform.position) < tolerance)
            {
                StartCoroutine(BossAttack());
                isAtP4 = true;
                isAtP3 = false;
                isAtP5 = false;

            }

            // Move to Point 5 
            if ((isAtP4 && !isAtP3 && bossHasAttacked) || (isAtP6 && hasReversed))
            {
                moveSpeed = currentSpeed;
                MoveToP5();
            }

            if (Vector2.Distance(transform.position, p5.transform.position) < tolerance)
            { 
                isAtP5 = true;
                isAtP4 = false;
                isAtP6 = false;
            }

            // Move to Point 6
            if ((isAtP5 && !isAtP4 && !hasReversed) || (isAtP7 && hasReversed))
            {
                MoveToP6();
            }

            if (Vector2.Distance(transform.position, p6.transform.position) < tolerance)
            {
                isAtP6 = true;
                isAtP5 = false;
                isAtP7 = false;
            }

            // Move to Point 7
            if ((isAtP6 && !isAtP5 && !hasReversed) || (isAtP8 && hasReversed))
            {
                MoveToP7();
            }

            if (Vector2.Distance(transform.position, p7.transform.position) < tolerance)
            {
                isAtP7 = true;
                isAtP6 = false;
                isAtP8 = false;
            }

            // Move to Point 8: When boss is at P7
            if (isAtP7 && !isAtP6 && !hasReversed)
            {
                MoveToP8();
            }

            if (Vector2.Distance(transform.position, p8.transform.position) < tolerance)
            {
                isAtP8 = true;
                isAtP7 = false;
                hasReversed = true;
                bossHasAttacked = false;
            }

        }

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
    }

    void MoveToP1()
    {
        transform.position = Vector2.MoveTowards(transform.position, p1.transform.position, moveSpeed * Time.deltaTime);
    }

    void MoveToP2()
    {
        transform.position = Vector2.MoveTowards(transform.position, p2.transform.position, moveSpeed * Time.deltaTime);
    }

    void MoveToP3()
    {
        transform.position = Vector2.MoveTowards(transform.position, p3.transform.position, moveSpeed * Time.deltaTime);
    }

    void MoveToP4()
    {
        transform.position = Vector2.MoveTowards(transform.position, p4.transform.position, moveSpeed * Time.deltaTime);
    }

    void MoveToP5()
    {
        transform.position = Vector2.MoveTowards(transform.position, p5.transform.position, moveSpeed * Time.deltaTime);
    }

    void MoveToP6()
    {
        transform.position = Vector2.MoveTowards(transform.position, p6.transform.position, moveSpeed * Time.deltaTime);
    }

    void MoveToP7()
    {
        transform.position = Vector2.MoveTowards(transform.position, p7.transform.position, moveSpeed * Time.deltaTime);
    }

    void MoveToP8()
    {
        transform.position = Vector2.MoveTowards(transform.position, p8.transform.position, moveSpeed * Time.deltaTime);
    }

    IEnumerator BossAttack()
    {
        moveSpeed = 0;
        
        if (timeBtwShot <= 0)
        {
            // shoot 
            ObjectPoolScript.spawnObject(bullet1, transform.position, Quaternion.identity);
            ObjectPoolScript.spawnObject(bullet2, transform.position, Quaternion.identity);
            ObjectPoolScript.spawnObject(bullet3, transform.position, Quaternion.identity);
            ObjectPoolScript.spawnObject(bullet4, transform.position, Quaternion.identity);
            ObjectPoolScript.spawnObject(bullet5, transform.position, Quaternion.identity);
            ObjectPoolScript.spawnObject(bullet6, transform.position, Quaternion.identity);
            timeBtwShot = levelTimeBtwShot;
        }
        else
        {
            timeBtwShot -= Time.deltaTime;
        }

        yield return new WaitForSeconds(5f);
        bossHasAttacked = true;
    }

    void UpdateStage()
    {
        if (health >= maxHealth * 0.75f)
        {
            //stage = 1;
            currentSpeed = L1Speed;
            moveSpeed = L1Speed;
            levelTimeBtwShot = L1ShootInterval;
        }
        else if (health >= maxHealth * 0.5f)
        {
            //stage = 2;
            currentSpeed = L2Speed;
            moveSpeed = L2Speed;
            levelTimeBtwShot = L2ShootInterval;
        }
        else if (health >= maxHealth * 0.25f)
        {
            //stage = 3;
            currentSpeed = L3Speed;
            moveSpeed = L3Speed;
            levelTimeBtwShot = L3ShootInterval;
        }
        else
        {
            //stage = 4;
            currentSpeed = L4Speed;
            moveSpeed = L4Speed;
            levelTimeBtwShot = L4ShootInterval;
        }


    }

    protected override void Move()
    {

    }

    protected override void TurnDirection()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (SBCollisionEvent != null)
            {
                SBCollisionEvent();
            }
        }
        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 15;
        }
    }
}
