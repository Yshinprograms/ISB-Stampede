using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] protected float moveSpeed;
    protected float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float distance;
    private SpriteRenderer sp;

    // The target is Piper
    protected Transform piper;

    void OnEnable()
    {
        health = maxHealth;
        piper = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sp = GetComponent<SpriteRenderer>();
        gameObject.tag = "Enemy";
        gameObject.layer = 6;
    }

    private void Start()
    {
        health = maxHealth;
        piper = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sp = GetComponent<SpriteRenderer>();
        gameObject.tag = "Enemy";
        gameObject.layer = 6;
    }

    private void Update()
    {
        Move();
        TurnDirection();

        if (health <= 0)
        {
            Death();
        }
    }

    protected virtual void Move()
    {
        // If the piper is within distance range, enemy will follow piper
        if (Vector2.Distance(transform.position, piper.position) < distance)
        {
            transform.position = Vector2.MoveTowards(transform.position, piper.position, moveSpeed * Time.deltaTime);
        }
       
    }

    private void TurnDirection()
    {
        if (transform.position.x > piper.position.x)
        {
            sp.flipX = true;
        }
        else
        {
            sp.flipX = false;
        }
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

}
