using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 1. Cleaner spawns from any of the 4 corners as usual
 * 2. Cleaner moves to random points all over the map, sweeping the floor 
 *     - does not engage piper automatically & does not take damage in this mode
 * 3. Once hit by one of Piper's projectiles, engage enraged mode:
 *     - Gets angry and "charges" up for 2s - Invincible in this state + animation & SFX
 *     - Charge towards Piper's position at the end of 2s - Furious sweeping animation
 * 4. If Cleaner is still alive, continue to charge towards Piper's position every 3s
 *     
 * Parameters: 10HP, 20DMG, 0.7 Speed, Does not take damage on first hit --> 5 Speed when charging */

public class Cleaner : Enemy
{
    public delegate void CleanerEvent();
    public static event CleanerEvent cleanerCollisionEvent;
    public static event CleanerEvent cleanerEnrageEvent;
    
    // Cleaner's aim line when charging towards Piper, adjust accordingly in coroutines
    public LineRenderer aimingLine;

    private bool enraged = false;
    private int projectileHitCount = 0;
    private Vector3 randomMapPosition = Vector3.zero;
    private bool isCharging = false;
    private bool completedEnrage = false;


    void OnEnable()
    {
        randomMapPosition = Vector3.zero;
        moveSpeed = 0.7f;
        health = maxHealth;
        projectileHitCount = 0;
        enraged = false;
        completedEnrage = false;
        isCharging = false;
        aimingLine.enabled = false;
    }

    protected override void Move()
    {
        // regular movement when not enraged
        if (!enraged)
        {
            // Either generate first randomPos or generate new randomPos after reaching the previous one
            if ((randomMapPosition == Vector3.zero) || (Vector3.Distance(transform.position, randomMapPosition) < 0.1f))
            {
                randomMapPosition = SpawnScript.generateMapPosition();
            }
            Vector3 directionToPosition = randomMapPosition - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, randomMapPosition, moveSpeed * Time.deltaTime);
            gameObject.GetComponent<Animator>().Play("CleanerMoving");
        }

        // Enraged movement pattern
        if (projectileHitCount == 1 && !enraged)
        {
            StartCoroutine(enterEnragedMode());
            enraged = true;
        }
        // Charging pattern after 1st charge
        else if (!isCharging && enraged && completedEnrage)
        {
            isCharging = true;
            StartCoroutine(continueCharge());
        }

    }

    public IEnumerator enterEnragedMode()
    {
        cleanerEnrageEvent();
        gameObject.GetComponent<Animator>().Play("CleanerIdle");
        // Get angry and charge up for 2s, then get Piper position and direction
        yield return new WaitForSeconds(2);
        Vector3 targetPosition = PiperScript.piperPosition;
        aimingLine.enabled = true;

        // Charge towards Piper's position
        moveSpeed = 5f;
        Vector3 directionToPiper = (targetPosition - transform.position).normalized;
        gameObject.GetComponent<Animator>().Play("CleanerRunning");
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            UpdateAimingLine(targetPosition);
            transform.position += moveSpeed * Time.deltaTime * directionToPiper;
            gameObject.GetComponent<Animator>().Play("CleanerRunning");
            yield return null;
        }

        // Stop, wait for next charge, completedEnrage sequence
        moveSpeed = 0;
        completedEnrage = true;
        aimingLine.enabled = false; // Hide the aiming line after charging
    }

    public IEnumerator continueCharge()
    {
        // Delay 3s in between each charge
        gameObject.GetComponent<Animator>().Play("CleanerIdle");
        yield return new WaitForSeconds(3);

        // Get Piper position and direction
        moveSpeed = 5f;
        Vector3 targetPosition = PiperScript.piperPosition;
        Vector3 directionToPiper = (targetPosition - transform.position).normalized;
        gameObject.GetComponent<Animator>().Play("CleanerRunning");
        aimingLine.enabled = true;

        // Charge towards Piper's position
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        //while (transform.position != targetPosition)
        {
            UpdateAimingLine(targetPosition);
            transform.position += moveSpeed * Time.deltaTime * directionToPiper;
            yield return null;
        }

        // Stop after reaching and reset coroutine
        moveSpeed = 0;
        isCharging = false;
        aimingLine.enabled = false; // Hide the aiming line after charging
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (cleanerCollisionEvent != null)
            {
                cleanerCollisionEvent();
            }
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.CompareTag("PaperBall"))
        {
            projectileHitCount += 1;
            // Does not take damage on first hit
            if (projectileHitCount > 1)
            { 
                health -= 10; 
            }
        }
    }

    // Function to update the aiming line renderer
    private void UpdateAimingLine(Vector3 targetPosition)
    {
        // Calculate the direction to the target
        Vector3 directionToPiper = (targetPosition - transform.position).normalized;

        // Set the aiming line's position
        aimingLine.SetPosition(0, transform.position);
        aimingLine.SetPosition(1, transform.position + (directionToPiper * Vector3.Distance(transform.position, targetPosition) ) );
    }
}
