using UnityEngine;

/* >>> Add the visual indicator of where the landmark is so players know where the tourists are going
 * 1. Spawns 2 at a time from a fixed point, max of 6 on the map at any point UNLESS Tour Bus has arrived 
 * 2. Goes towards designated landmarks on level 3 map >> L3LogicScript 
 * 3. Takes a photo at the 6s mark && reachedLandmark, add flash animation at 6s >> ChineseTouristScript 
 * 4. Designated landmarks rotate every 7s, so ALL tourists move towards next landmark (can add "zai na'r" voiceline for fun)
 * 5. If Piper gets within range of tourist when they move, tourists in range will move towards Piper ("Saurry, du yuu sapeek Chinese?" voiceline) --> range = 6f, move only if in range
 * 6. If Piper gets within range of tourist camera, tourist takes photo with flash --> range = 1f (adjust based on Piper's range)
 *      - Stun Piper for 0.6s
 *      - Tourist pauses for 1.8s
 * 7. Repeat from step 2

 * Parameters: 10HP, 0DMG, 2 Speed, Spawn every 5s, No projectile
 */

public class ChineseTourist : Enemy
{
    public delegate void CTEvent();
    public static event CTEvent PhotoEvent;
    public static event CTEvent CTCollisionEvent;

    public static int touristCount;
    public static Vector3 landmark;

    private float distToPiper;
    private float touristToPiperRange;
    private float flashRange;
    private float photoTakenTimer;
    private bool photoTaken;

    Animator animCT; 

    private void OnEnable()
    {
        health = maxHealth;
        moveSpeed = 2f;
        photoTakenTimer = 0f;
        photoTaken = false;
        animCT = GetComponent<Animator>();
    }
    void Start()
    {
        touristToPiperRange = 4f;
        flashRange = 1.1f;
        health = maxHealth;
        photoTakenTimer = 0f;
        CTCollisionEvent += CTInflictDamage;
        photoTaken = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, landmark) < 1.5f)
        {
            animCT.Play("ChineseTouristIdle");
        }

        // Find distance between Aunty and Piper
        distToPiper = Vector3.Distance(transform.position, PiperScript.piperPosition);
        animCT.Play("ChineseTouristWalking");

        if (photoTaken && (photoTakenTimer > 1.8f))
        {
            // CT can move again after 2s delay from taking photo of Piper
            photoTaken = false;
            moveSpeed = 2f;
        }
        else
        {
            // Move towards Piper if within range, else move towards the landmark which changes every 7s
            if (distToPiper - touristToPiperRange <= 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, PiperScript.piperPosition, moveSpeed * Time.deltaTime);
                if (transform.position.x > PiperScript.piperPosition.x)
                {
                    transform.localScale = new Vector2(-0.15f, 0.15f);
                }
                else
                {
                    transform.localScale = new Vector2(0.15f, 0.15f);
                }
                animCT.Play("ChineseTouristWalking");
            }
            // Ensures CT doesn't cluster together
            else if (Vector3.Distance(transform.position, landmark) > 1.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, landmark, moveSpeed * Time.deltaTime);
                if (transform.position.x > landmark.x)
                {
                    transform.localScale = new Vector2(-0.15f, 0.15f);
                }
                else
                {
                    transform.localScale = new Vector2(0.15f, 0.15f);
                }
                animCT.Play("ChineseTouristWalking");
            }

            // Trigger flash sequence when distance <= 1
            if (distToPiper - flashRange <= 0)
            {
                TakePhoto();
                // PiperStunned is subscribed here
                if (!PiperScript.malaActive)
                {
                    PhotoEvent();
                }
                animCT.Play("ChineseTouristTakingPhoto");

            }

        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        photoTakenTimer += Time.deltaTime;
        if (health <= 0)
        {
            touristCount -= 1;
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Invokes if not null
            CTCollisionEvent?.Invoke();
        }

        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.CompareTag("PaperBall"))
        {
            health -= 10;
        }
    }

    void TakePhoto()
    {
        photoTaken = true;
        photoTakenTimer = 0;
        moveSpeed = 0f;
    }

    void CTInflictDamage()
    {
        PiperScript.piperHealth -= 5;
    }

    private void OnDestroy()
    {
        PhotoEvent -= TakePhoto;
        CTCollisionEvent -= CTInflictDamage;
    }
}
