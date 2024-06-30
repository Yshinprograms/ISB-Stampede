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
                    GameObject go = new("L3LogicManager");
                    instance = go.AddComponent<L3LogicScript>();
                }
            }
            return instance;
        }
    }

    public bool bossBattle;
    private float levelTimer = 0;

    // Import GameObjects, drag and drop into Inspector
    public GameObject chineseTourist;
    public GameObject landmark;
    public GameObject med;
    public GameObject innocentStudent;
    public GameObject bizSnake;
    public GameObject paperBall;
    public GameObject ChineseTourBus;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenChineseTouristSpawn = 5f;
    public float secondsBetweenLandmarks = 7;
    public float secondsBetweenInnocentStudentSpawn = 8f;
    //public float secondsBetweenMedStudentSpawn = 5f;
    public float secondsBetweenBizSnakeSpawn = 8f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;

    void Awake()
    {
        Instantiate(landmark, ChineseTourist.landmark, transform.rotation);

        // Ensure only 1 instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;


        // Start spawning
        InvokeRepeating(nameof(SpawnChineseTourists), 0f, secondsBetweenChineseTouristSpawn);

        InvokeRepeating(nameof(SpawnInnocentStudent), 0f, secondsBetweenInnocentStudentSpawn);

        ChineseTourBusScript.BusCollisionEvent += BusInflictDamage;

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

        // Boss Spawns 1 time when timer hits 180s
        if (levelTimer > 3 && !bossBattle)
        {
            bossBattle = true;
            ChineseTourBus.SetActive(true);
        }


        levelTimer += Time.deltaTime;
    }

    void BizProjectileInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }

    // Spawn 2 ChineseTourists every 5s, max 6 tourists
    void SpawnChineseTourists()
    {
        if (ChineseTourist.touristCount < 6)
        {
            ChineseTourist.touristCount += 2;
            ObjectPoolScript.spawnObject(chineseTourist, SpawnScript.generateSpawnPoint(), Quaternion.identity);
            ObjectPoolScript.spawnObject(chineseTourist, SpawnScript.generateSpawnPoint(), Quaternion.identity);
        }
    }
    void SpawnInnocentStudent()
    {
        ObjectPoolScript.spawnObject(innocentStudent, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void SpawnPaperBall()
    {
        // Spawn to the right of Piper
        ObjectPoolScript.spawnObject(paperBall, PiperScript.piperPosition + Vector3.right, Quaternion.identity);
    }

    // !!!Change this function for gameend!!!
    public void LevelCompleted()
    {
        //gameScreenManager.GoToLevel3();
    }

    void BusInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(SpawnChineseTourists));
        CancelInvoke(nameof(SpawnInnocentStudent));
        ChineseTourBusScript.BusCollisionEvent -= BusInflictDamage;
    }
}
