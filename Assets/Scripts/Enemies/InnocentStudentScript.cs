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
*/

// No need to inherit enemy class
// No need for maxHealth, because only dies upon collision with other enemies
public class InnocentStudentScript : MonoBehaviour
{
    public GameObject bizSnake;

    private float moveSpeed = 1f;
    private float pushBackTimer = 3f;
    private float stunTimer = 3f;
    private float transformationTimer;
    private bool latchedOn = false;
    private bool collidedWithPiper = false;
    private Vector2 pushBackDir;

    private void OnEnable()
    {
        moveSpeed = 1f;
        latchedOn = false;
        collidedWithPiper = false;
        transformationTimer = 0f;
        pushBackTimer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        // Move alongside Piper if latchedOn
        if (latchedOn)
        {
            if (Vector2.Distance(transform.position, PiperScript.piperPosition) > 0.8f)
            {
                moveSpeed = PiperScript.piperMoveSpeed;
                transform.position = Vector2.MoveTowards(transform.position, PiperScript.piperPosition, Time.deltaTime * moveSpeed);
                transformationTimer += Time.deltaTime;
            }

            // Transform into BizSnake after 5s, reset PiperSpeed
            if (transformationTimer > 5)
            {
                PiperScript.piperMoveSpeed = 5f;
                ObjectPoolScript.spawnObject(bizSnake, transform.position, transform.rotation);
                ObjectPoolScript.returnObjectToPool(gameObject);
            }
        }
        // Getting pushed by Piper, must be before getting stunned
        else if (pushBackTimer < 0.2)
        {
            transform.position += 6f * Time.deltaTime * (Vector3)pushBackDir;
            pushBackTimer += Time.deltaTime;
            stunTimer = 0f;
        }
        // Stunned after the pushback; Stunned when timer < 2
        else if (stunTimer < 2)
        {
            // stunned animation
            stunTimer += Time.deltaTime;
        }
        // Regular movement towards Piper
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, PiperScript.piperPosition, Time.deltaTime * moveSpeed);
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only check for collisions with Piper or enemies, if IS has not contacted Piper yet and not Stunned
        if (!collidedWithPiper && (stunTimer > 2))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collidedWithPiper = true;
                latchedOn = true;
                PiperScript.piperMoveSpeed = 3f;
            }
        }
        // Destroyed if collision with enemy and being pushed back (Layers)
        if ((collision.gameObject.layer == 6 || collision.gameObject.layer == 11) && (pushBackTimer < 0.2))
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }

    // Call this when hit by Piper's push
    public void PushBack()
    {
        pushBackDir = (-(PiperScript.piperPosition - transform.position)).normalized;
        pushBackTimer = 0f;
    }
}
