using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS1010Script : Enemy
{
    public Sprite[] stages;
    public GameObject cs1010Projectile;
    //public BossHealthbarScript bossHealthbarScript;

    private SpriteRenderer cs1010renderer;
    private bool stageZero;
    private bool stageOne;
    private bool stageTwo;
    private bool stageThree;
    private bool stageOneinProgress;
    private bool stageTwoinProgress;
    private float projectileTimer;

    private float shootDelay = 0.35f;
    private float patternDelay = 2.5f;

    // Required for game restart despite only spawing a single CS1010 in the whole game
    private void OnEnable()
    {
       // bossHealthbarScript.EnableHealthbar();
       // bossHealthbarScript.SetMaxHealth(maxHealth);
        cs1010renderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        stageZero = true;
        stageOne = false;
        stageTwo = false;
        stageThree = false;
        stageOneinProgress = false;
        stageTwoinProgress = false;
        projectileTimer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
      //  bossHealthbarScript.SetHealth(health);
        UpdateStage();
        Attack();

        if (health <= 0)
        {
           // bossHealthbarScript.DisableHealthbar();
           // bossHealthbarScript = null;
            ObjectPoolScript.returnObjectToPool(gameObject);
            L2LogicScript.Instance.bossBattle = false;
            L2LogicScript.Instance.LevelCompleted();
        }
    }

    void UpdateStage()
    {
        if (health >= maxHealth * 0.75f)
        {
            cs1010renderer.sprite = stages[0];
            stageZero = true;
        }
        else if (health >= maxHealth * 0.5f)
        {
            cs1010renderer.sprite = stages[1];
            stageZero = false;
            stageOne = true;
        }
        else if (health >= maxHealth * 0.25f)
        {
            cs1010renderer.sprite = stages[2];
            stageOne = false;
            stageTwo = true;
        }
        else
        {
            cs1010renderer.sprite = stages[3];
            stageTwo = false;
            stageThree = true;
        }
    }

    void Attack()
    {
        if (stageZero && (projectileTimer > 3f))
        {
            // CS1010 will shoot a projectile every 3 seconds
            ObjectPoolScript.spawnObject(cs1010Projectile, transform.position, Quaternion.identity);
            projectileTimer = 0f;
        }
        else if (stageOne && !stageOneinProgress)
        {
            // PE0 will shoot 2 projectiles consecutively every 3 seconds
            StartCoroutine(StageOneSequence());
        }
        else if (stageTwo && !stageTwoinProgress)
        {
            // PE0 will shoot 3 projectiles consecutively every 3 seconds
            StartCoroutine(StageTwoSequence());
        }
        else if (stageThree && (projectileTimer > 0.7f))
        {
            // CS1010 will shoot a projectile every .7 seconds
            ObjectPoolScript.spawnObject(cs1010Projectile, transform.position, Quaternion.identity);
            projectileTimer = 0f;
        }
        projectileTimer += Time.deltaTime;
    }
    private IEnumerator StageOneSequence()

    {
        stageOneinProgress = true;
        yield return new WaitForSeconds(patternDelay);

        ObjectPoolScript.spawnObject(cs1010Projectile, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(shootDelay);

        ObjectPoolScript.spawnObject(cs1010Projectile, transform.position, Quaternion.identity);

        stageOneinProgress = false;
    }

    private IEnumerator StageTwoSequence()
    {
        stageTwoinProgress = true;
        yield return new WaitForSeconds(patternDelay);

        ObjectPoolScript.spawnObject(cs1010Projectile, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(shootDelay);

        ObjectPoolScript.spawnObject(cs1010Projectile, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(shootDelay);

        ObjectPoolScript.spawnObject(cs1010Projectile, transform.position, Quaternion.identity);

        stageTwoinProgress = false;
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