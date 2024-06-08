using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L3LogicScript : MonoBehaviour
{
    private static L3LogicScript instance;
    public static L3LogicScript Instance
    {
        // get accesses the single instance of this script when other classes try to access it
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<L3LogicScript>();
                if (instance == null)
                {
                    GameObject go = new GameObject("L3LogicManager");
                    instance = go.AddComponent<L3LogicScript>();
                }
            }
            return instance;
        }
    }

    // Import GameObjects, drag and drop into Inspector
    public GameObject chineseTourist;
    public GameObject med;
    public GameObject bizSnake;
    public GameObject paperBall;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenChineseTouristSpawn = 5f;
    public float secondsBetweenLandmarks;
    //public float secondsBetweenMedStudentSpawn = 5f;
    public float secondsBetweenBizSnakeSpawn = 8f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;

    void Awake()
    {
        // Ensure only 1 instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;


        // Start spawning
        //Bollard.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating(nameof(SpawnChineseTourists), 0f, secondsBetweenChineseTouristSpawn);

        //// Freshie interaction and Spawns ; time 60s
        //Freshie.freshieCollisionEvent += freshieInflictDamage;
        //InvokeRepeating("spawnFreshie", 60f, secondsBetweenFreshieSpawn);

        PaperBallScript.activePaperBalls = 0;
    }

    void Update()
    {
        // Piper's projectile interactions
        secondsBetweenPaperBallSpawn += Time.deltaTime;
        if ((PaperBallScript.activePaperBalls < maxActivePaperBalls) && (secondsBetweenPaperBallSpawn > 1))
        {
            SpawnPaperBall();
            secondsBetweenPaperBallSpawn = 0;
            PaperBallScript.activePaperBalls += 1;
        }

        // Rotate landmarks for ChineseTourists
        secondsBetweenLandmarks += Time.deltaTime;
        if (secondsBetweenLandmarks > 7)
        {
            ChineseTourist.landmark = SpawnScript.generateMapPosition();
            secondsBetweenLandmarks = 0;
        }
        Debug.Log(ChineseTourist.touristCount.ToString());
    }

    void bizProjectileInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }

    // Spawn 2 ChineseTourists at a time, max 6 tourists, potentially 10s or more between spawns
    void SpawnChineseTourists()
    {
        if (ChineseTourist.touristCount < 6)
        {
            ChineseTourist.touristCount += 2;
            ObjectPoolScript.spawnObject(chineseTourist, SpawnScript.generateSpawnPoint(), Quaternion.identity);
            ObjectPoolScript.spawnObject(chineseTourist, SpawnScript.generateSpawnPoint(), Quaternion.identity);
        }
    }
    void SpawnBizSnake()
    {
        ObjectPoolScript.spawnObject(bizSnake, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void SpawnPaperBall()
    {
        // Spawn to the right of Piper
        ObjectPoolScript.spawnObject(paperBall, PiperScript.piperPosition + Vector3.right, Quaternion.identity);
    }


    private void OnDestroy()
    {
        CancelInvoke(nameof(SpawnChineseTourists));
        CancelInvoke(nameof(SpawnBizSnake));
        //CSMugger.csMuggerCollisionEvent -= csMuggerInflictDamage;
    }
}
