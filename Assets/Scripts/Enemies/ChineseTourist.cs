using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* >>> Make sure to designate and design landmarks on the level 3 map (via gameObjects for simplicity?)
 * 1. Spawns 2 at a time from a fixed point, max of 6 on the map at any point UNLESS Tour Bus has arrived 
 * 2. Goes towards designated landmarks on level 3 map >> L3LogicScript
 * 3. Takes a photo at the 6s mark, add flash animation at 6s >> ChineseTouristScript
 * 4. Designated landmarks rotate every 7s, so ALL tourists move towards next landmark (can add "zai na'r" voiceline for fun)
 * 5. If Piper gets within range of tourist when they move, tourists in range will move towards Piper ("Saurry, du yuu sapeek Chinese?" voiceline) --> range = 6f, move only if in range
 * 6. If Piper gets within range of tourist camera, tourist takes photo with flash --> range = 1f (adjust based on Piper's range), stun Piper for 0.5s
 * 7. Delay for 2s after taking photo --> repeat from step 2
 * Optionally, add the visual indicator of where the landmark is so players know where the tourists are going

 * Parameters: 10HP, 0DMG, 2 Speed, Spawn every 5s, No projectile
 */

public class ChineseTourist : Enemy
{
    public static int touristCount;
    public static Vector3 landmark;

    private float distToPiper;
    private float touristRange;
    private float flashRange;

    private void OnEnable()
    {
        health = maxHealth;
    }
    void Start()
    {
        health = maxHealth;
        touristRange = 6f;
        flashRange = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Find distance between Aunty and Piper
        distToPiper = Vector3.Distance(transform.position, PiperScript.piperPosition);

        // Move towards Piper if within range, else move towards the landmark which changes every 7s
        if (distToPiper - touristRange <= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, PiperScript.piperPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, landmark, moveSpeed * Time.deltaTime);
        }

        // Trigger flash sequence when distance <= 1
        if (distToPiper - flashRange <= 0)
        {
            moveSpeed = 0;
            //flash();
        }
    }
}
