using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* >>> InnocentStudent GameObject (Impervious to Piper projectiles, NOT enemy)
* Move towards Piper slowly
* Upon contact with Piper, "Do you want to group up?"
* When contacted, latches on to Piper
*   - When latched on, piperMoveSpeed reduced to 3f
*   - IS moveSpeed changed to reducedPiperMoveSpeed to maintain position
*   - Unable to get pushed away now
* After 5s, turns into 'BizSnake'
* 
* Piper push:(Piper gains a new ability to push or shoo away innocent students by pressing 'e' when within range. --> Might need to include tutorial in level 3 cutscene or upon level start)
* - IS pushed back by 2f
*   - If contacted with any other enemies, get destroyed
*   - If not contacted, stunned for 3s
* Parameters: *HP, 0DMG, 1.5 Speed, Spawn every 8s, No projectile
* 
* >>> BizSnake GameObject (Enemy)
* 2s delay for Piper to run away after transforming into BizSnake
* Position is fixed, launches toxic words at Piper no matter where she is
* Shouts "Don't burden leh" with every projectile
* 
* Parameters: 20HP, 10DMG, 0 Speed, No Spawn, Shoot every 3s
*/

public class BizSnake : Enemy
{
    public delegate void BizSnakeEvent();
    public static event BizSnakeEvent TransformEvent;

    public GameObject bizSnakeProjectile;

    private float throwTimer = 0f;

    private void OnEnable()
    {
        TransformEvent();
        health = maxHealth;
    }

    void Update()
    {
        if (throwTimer > 3f)
        {
            throwTimer = 0f;
            ObjectPoolScript.spawnObject(bizSnakeProjectile, transform.position + Vector3.up, Quaternion.identity);
        }

        if (health <= 0)
        {
            Death();
        }

        throwTimer += Time.deltaTime;
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
