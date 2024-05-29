using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 1. EnginKid spawns from any of the 4 sides as usual
 * 2. EnginKid ventures into map and looks for other engin kids --> non-Attack phase
 * 3. If no other enginkids, go hide in the corner -- > non-Attack phase
 * 4. If no. of enginkids on map >= 3 && together, go towards Piper --> Attack phase
 * 5. Stop spawning enginkids until all 3 are gone
 *      5a. If any one enginKid dies otw to gathering point, others immediately go into attack mode
 * 6. enginkids have bad hygiene and BO --> damaging aura 
 * 7. If enginKids gather successfully, the radius of their aura increases from 1 to 1.5
 * 
 * Parameters: 20HP, 10DMG/s, 2.5 Speed
 */

public class EnginKidScript : MonoBehaviour
{
    public static float auraDistance;
    public static bool attackPhase = false;
    public static int enginKidCount = 0;
    public static bool gatheredSuccessfully;
    public delegate void EnginEvent();
    public static event EnginEvent enginKidDeathEvent;
    public float enginSpeed;
    public int maxHealth = 20;
    public int health;

    private bool reachedGatheringCorner;
    private float lastDamageTime = 0;
    private bool piperInAuraRange;
    private ParticleSystem aura;

    void Start()
    {
        health = maxHealth;
        reachedGatheringCorner = false;
        enginSpeed = 2.5f;      
        gatheredSuccessfully = false;
        aura = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        health = maxHealth;
        reachedGatheringCorner = false;
        enginSpeed = 2.5f;
        gatheredSuccessfully = false;
    }
    void Update()
    {
        // Constantly check if Piper is in range of the damaging aura
        piperInAuraRange = isInRange();

        // Go to the gathering corner until we get a cluster of 3 enginKids
        if (!attackPhase && !reachedGatheringCorner)
        {
            transform.position = Vector3.MoveTowards(transform.position, LogicScript.enginKidGatheringCorner, enginSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, LogicScript.enginKidGatheringCorner) < 0.6f)
            {
                reachedGatheringCorner = true;
            }
        }
        // Go into attackPhase when the cluster of 3 is formed i.e. all 3 reached position
        else if (attackPhase)
        {
            transform.position = Vector3.MoveTowards(transform.position, PiperScript.piperPosition, enginSpeed * Time.deltaTime);
        }

        if (health <= 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
            enginKidCount -= 1;
            // Only triggers checking whether no enginKids exist on map whenever one is returned to pool
            if (enginKidCount == 0 )
            {
                enginKidDeathEvent();
                LogicScript.enginKidClusterActive = false;
                attackPhase = false;
            }
            // If any one of the enginKid dies before all 3 assemble, stop spawning(coroutine) and the other enginKids enter attack phase
            // When the last enginKid dies, will go into the condition above, and conditions for cluster spawning reset
            else
            {
                enginKidDeathEvent();
                attackPhase = true;
            }
        }

        /* Damage mechanism:
         * 1. Upon contact, instantly deduct 10HP from Piper
         * 2. Every 1s that Piper stays within the aura, deduct another 10HP 
         */
        if (piperInAuraRange && (Time.time - lastDamageTime >= 1f))
        {
            LogicScript.piperHealth -= 10;
            lastDamageTime = Time.time;
        }

        // Activate group boost
        if (gatheredSuccessfully)
        {
            var mainModule = aura.main;  // Get the main module
            mainModule.startLifetimeMultiplier = 0.48f;
            mainModule.startSpeedMultiplier = 0.75f;
            auraDistance = 1.5f;
        }
        else
        {
            var mainModule = aura.main;
            mainModule.startLifetimeMultiplier = 0.34f;
            mainModule.startSpeedMultiplier = 0.58f;
            auraDistance = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

        }

        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }
    }

    bool isInRange()
    {
        return (Vector3.Distance(PiperScript.piperPosition, transform.position) <= auraDistance);
    }
}
