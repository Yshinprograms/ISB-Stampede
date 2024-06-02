using UnityEngine;

// CSMugger starts with 20 Health

public class CSMugger : Enemy
{
    public delegate void CSMuggerCollision();
    public static event CSMuggerCollision csMuggerCollisionEvent;

    private float timeBtwCode;
    public float startTimeBtwCode = 10;
    private float codeCount = 0;
    public float maxCodeProduce = 3f;

    public GameObject csMuggerCode;

    void OnEnable()
    {
        health = maxHealth;
        timeBtwCode = startTimeBtwCode;
    }

    protected override void Move()
    {
        base.Move();

        // Set up code frequency
        if (codeCount < maxCodeProduce)
        {
            if (timeBtwCode <= 0)
            {
                ObjectPoolScript.spawnObject(csMuggerCode, transform.position, Quaternion.identity);
                timeBtwCode = startTimeBtwCode;
                codeCount += 1;
            }
            else
            {
                timeBtwCode -= Time.deltaTime;
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (csMuggerCollisionEvent != null)
            {
                csMuggerCollisionEvent();
            }
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }
    }
}
