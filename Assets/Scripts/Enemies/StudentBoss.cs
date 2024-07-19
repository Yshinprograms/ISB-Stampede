using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

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


    private void OnEnable()
    {      
        bossHealthbarScript.EnableHealthbar();
        bossHealthbarScript.SetMaxHealth((int)maxHealth);
        health = maxHealth;

    }

    void Update()
    {
        bossHealthbarScript.SetHealth((int)health);
        Move();

        if (health <= 0)
        {
            bossHealthbarScript.DisableHealthbar();
            gameObject.SetActive(false);
            L1LogicScript.Instance.bossBattle = false;
            L1LogicScript.Instance.LevelCompleted();
        }
    }

    protected override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, p0.transform.position, moveSpeed * Time.deltaTime);
        //Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == p0)
        {
            transform.position = Vector2.MoveTowards(transform.position, p1.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    protected override void TurnDirection()
    {
        if (transform.position.x > PiperScript.piperPosition.x)
        {
            transform.localScale = new Vector2(-0.35f, 0.35f);
        }
        else
        {
            transform.localScale = new Vector2(0.35f, 0.35f);
        }
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
