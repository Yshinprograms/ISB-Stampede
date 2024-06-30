using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Watch out for overspawning of CT

public class ChineseTourBusScript : Enemy
{
    public delegate void BusEvent();
    public static event BusEvent BusCollisionEvent;
    public GameObject chineseTourist;
    public Sprite[] sprites;
    public BossHealthbarScript bossHealthbarScript;
    public Text bossHealth;

    private SpriteRenderer busRenderer;
    //private int stage;
    private int currentCorner;
    private Vector2 destination;
    private bool tourStarted;
    private Vector3 originalScale = new(3, 3, 3);
    private Vector3 flipScale = new(-3, 3, 3);
    private float touristSpawnDelay;

    private void OnEnable()
    {
        bossHealthbarScript.EnableHealthbar();
        bossHealthbarScript.SetMaxHealth((int)maxHealth);
        busRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        currentCorner = 1;
        tourStarted = false;
        bossHealth.text = "Chinese Tour Bus";
    }

    void Update()
    {
        bossHealthbarScript.SetHealth((int)health);
        UpdateStage();
        GetDestination(currentCorner);
        if (!tourStarted)
        {
            StartCoroutine(StartTour());
        }

        if (health <= 0)
        {
            bossHealthbarScript.DisableHealthbar();
            gameObject.SetActive(false);
            //L3LogicScript.Instance.bossBattle = false;
            L3LogicScript.Instance.LevelCompleted();
        }
    }

    void UpdateStage()
    {
        if (health >= maxHealth * 0.75f)
        {
            //stage = 1;
            moveSpeed = 2f;
            touristSpawnDelay = 1f;
        }
        else if (health >= maxHealth * 0.5f)
        {
            //stage = 2;
            moveSpeed = 3f;
            touristSpawnDelay = 0.85f;
        }
        else if (health >= maxHealth * 0.25f)
        {
            //stage = 3;
            moveSpeed = 4f;
            touristSpawnDelay = 0.7f;
        }
        else
        {
            //stage = 4;
            moveSpeed = 5f;
            touristSpawnDelay = 0.6f;
        }
    }


    // Updates bus destination and update sprite and collider
    // bus faces Bottom Left by default, if going right, flipScale
    void GetDestination(int currentCorner)
    {
        switch (currentCorner)
        {
            // Bus starts TopRight
            case 1: // Top Right --> Bottom Left
                destination = new Vector2(-SpawnScript.xEnginGatheringCorner, SpawnScript.yLowerEnginGatheringCorner);
                transform.localScale = originalScale;
                busRenderer.sprite = sprites[0];
                break;
            case 2: // BL > TL
                destination = new Vector2(-SpawnScript.xEnginGatheringCorner, SpawnScript.yUpperEnginGatheringCorner);
                transform.localScale = originalScale;
                busRenderer.sprite = sprites[2];
                break;
            case 3: // TL > BR
                destination = new Vector2(SpawnScript.xEnginGatheringCorner, SpawnScript.yLowerEnginGatheringCorner);
                transform.localScale = flipScale;
                busRenderer.sprite = sprites[0];
                break;
            case 4: // BR > TR
                destination = new Vector2(SpawnScript.xEnginGatheringCorner, SpawnScript.yUpperEnginGatheringCorner);
                transform.localScale = flipScale;
                busRenderer.sprite = sprites[2];
                break;
            default:
                Debug.LogError("Invalid side value in generateSpawnPoint()");
                break;
        }
    }

    IEnumerator StartTour()
    {
        tourStarted = true;
        //travelling = true;
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }

        currentCorner += 1;

        if (currentCorner > 4)
        {
            currentCorner = 1;
        }

        for (int i = 0; i < 4; i += 1)
        {
            ObjectPoolScript.spawnObject(chineseTourist, transform.position, chineseTourist.transform.rotation);
            yield return new WaitForSeconds(touristSpawnDelay);
        }

        tourStarted = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Figure out how to put this into the paperBall script instead
        if (collision.gameObject.layer == 7)
        {
            health -= 10;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            BusCollisionEvent?.Invoke();
        }
    }
}
