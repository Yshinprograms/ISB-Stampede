using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName;
    [SerializeField] protected float moveSpeed;
    protected float health;
    [SerializeField] public float maxHealth;
    //private SpriteRenderer sp;

    // The target is Piper
    protected Transform piper;
    Animator anim;

    void OnEnable()
    {
        health = maxHealth;
        //sp = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        gameObject.tag = "Enemy";
        gameObject.layer = 6;
    }

    /*private void Start()
    {
        health = maxHealth;
        //sp = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        gameObject.tag = "Enemy";
        gameObject.layer = 6;
    }*/

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
        transform.position = Vector2.MoveTowards(transform.position, PiperScript.piperPosition, moveSpeed * Time.deltaTime);
    }

    protected virtual void TurnDirection()
    {
        if (transform.position.x > PiperScript.piperPosition.x)
        {
           transform.localScale = new Vector2(-0.15f, 0.15f);
        }
        else
        {
            transform.localScale = new Vector2(0.15f, 0.15f);
        }
    }

    protected virtual void Death()
    {
        //Destroy(gameObject);
        ObjectPoolScript.returnObjectToPool(gameObject);
    }

}
