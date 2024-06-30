using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bollard, Freshie, Aunty

public class L1LogicScript : MonoBehaviour
{
    private float timer = 0;

    // Import Game Manager Script & Game end conditions
    public GameScreenManager gameScreenManager;

    // Import GameObjects, drag and drop into Inspector
    public GameObject testObject;
    public GameObject testObject2;
    public GameObject paperBall;
    public GameObject bollard;
    public GameObject freshie;
    public GameObject aunty;
    

    // Spawn times
    //public float secondsBetweenTestObjectSpawn = 10f;
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenBollardSpawn = 4f;
    public float secondsBetweenFreshieSpawn = 6f;
    public float secondsBetweenAuntySpawn = 6f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;

    // Singleton Implementation
    private static L1LogicScript instance;
    public static L1LogicScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<L1LogicScript>();
                if (instance == null)
                {
                    GameObject go = new("LogicManager");
                    instance = go.AddComponent<L1LogicScript>();
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        // Ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        // Test interaction and Spawns ; time 0s
        //InvokeRepeating(nameof(SpawnTest), 0f, secondsBetweenTestObjectSpawn);

        // Bollard interaction and Spawns ; time 0s
        Bollard.bollardCollisionEvent += BollardInflictDamage;
        InvokeRepeating(nameof(SpawnBollard), 0f, secondsBetweenBollardSpawn);

        // Freshie interaction and Spawns ; time 60s
        Freshie.freshieCollisionEvent += FreshieInflictDamage;
        InvokeRepeating(nameof(SpawnFreshie), 60f, secondsBetweenFreshieSpawn);

        // Aunty interactions and spawns ; time 120s
        Aunty.auntyCollisionEvent += AuntyInflictDamage;
        Handbag.handbagCollisionEvent += HandbagInflictDamage;
        InvokeRepeating(nameof(SpawnAunty), 120f, secondsBetweenAuntySpawn);

        // Piper's parameters & projectile interactions
        //isAlive = true;
        PaperBallScript.activePaperBalls = 0;
    }

    void Update()
    {

        /*if (PiperScript.piperHealth <= 0 && isAlive)
        {
            isAlive = false;
            gameScreenManager.gameOver();
            Debug.Log("Dead");
        }*/

        // Piper's projectile interactions
        secondsBetweenPaperBallSpawn += Time.deltaTime;
        if ((PaperBallScript.activePaperBalls < maxActivePaperBalls) && (secondsBetweenPaperBallSpawn > 1))
        {
            SpawnPaperBall();
            secondsBetweenPaperBallSpawn = 0;
            PaperBallScript.activePaperBalls += 1;
        }

        if (timer > 1)
        {
            gameScreenManager.GoToLevel2();
            PowerUpManagerScript.Instance.levelTwo = true;
            //gameScreenManager.GameCompleted();
        }

        secondsBetweenPaperBallSpawn += Time.deltaTime;
        timer += Time.deltaTime;
    }

    // Damages
    void BollardInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }
    void FreshieInflictDamage()
    {
        PiperScript.piperHealth -= 20;
    }
    void AuntyInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }
    void HandbagInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }
    // Spawn Enemies
    void SpawnBollard()
    {
        ObjectPoolScript.spawnObject(bollard, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void SpawnFreshie()
    {
        ObjectPoolScript.spawnObject(freshie, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void SpawnAunty()
    {
        ObjectPoolScript.spawnObject(aunty, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void SpawnPaperBall()
    {
        // Spawn to the right of Piper
        ObjectPoolScript.spawnObject(paperBall, PiperScript.piperPosition + Vector3.right, Quaternion.identity);
    }

    // Unsubscribe from Events
    private void OnDestroy()
    {
        CancelInvoke(nameof(SpawnBollard));
        CancelInvoke(nameof(SpawnFreshie));
        CancelInvoke(nameof(SpawnAunty));
        Bollard.bollardCollisionEvent -= BollardInflictDamage;
        Freshie.freshieCollisionEvent -= FreshieInflictDamage;
        Aunty.auntyCollisionEvent -= AuntyInflictDamage;
        Handbag.handbagCollisionEvent -= HandbagInflictDamage;
    }



    void SpawnTest()
    {
        ObjectPoolScript.spawnObject(testObject, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
}
