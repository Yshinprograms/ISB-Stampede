using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Cleaner, EnginKid, CSMugger

public class L2LogicScript : MonoBehaviour
{
    private float levelTimer = 0;
    public bool bossBattle;

    // Import Game Manager Script & Game end conditions
    public GameScreenManager gameScreenManager;

    // Import GameObjects, drag and drop into Inspector
    public GameObject paperBall;
    public GameObject cleaner;
    public GameObject enginKid;
    public GameObject csMugger;
    public GameObject csMuggerCode;
    public GameObject cs1010;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    private float secondsBetweenCleanerSpawn = 5f;
    private float secondsBetweenEnginKidSpawn = 5f;
    private float secondsBetweenCSMuggerSpawn = 5f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;

    // EnginKid Logic
    public bool enginKidClusterActive;
    public Vector3 enginKidGatheringCorner;
    private Coroutine EnginKidClusterCoroutine;
    public delegate void EnginEvent();
    public static event EnginEvent EnginKidGatheredEvent;

    // Singleton Implementation
    private static L2LogicScript instance;
    public static L2LogicScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<L2LogicScript>();
                if (instance == null)
                {
                    GameObject go = new GameObject("LogicManager");
                    instance = go.AddComponent<L2LogicScript>();
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

        // Set timer for Level 2
        TimerScript.remainingTime = 10;

        // Cleaner interaction and Spawns ; time 0s
        Cleaner.cleanerCollisionEvent += CleanerInflictDamage;
        InvokeRepeating(nameof(SpawnCleaner), 600f, secondsBetweenCleanerSpawn);

        // EnginKid interaction and Spawns
        EnginKid.enginKidDeathEvent += StopEnginKidClusterCoroutine;
        enginKidClusterActive = false;

        // CSMugger interaction and Spawns ; time 120s
        CSMugger.csMuggerCollisionEvent += CSMuggerInflictDamage;
        CSMuggerCode.csMuggerCodeCollisionEvent += CSMuggerCodeInflictDamage;
        InvokeRepeating(nameof(SpawnCSMugger), 600f, secondsBetweenCSMuggerSpawn);

        // Piper's parameters & projectile interactions
        PaperBallScript.activePaperBalls = 0;

        CS1010ProjectileScript.CS1010ProjectileCollisionEvent += CS1010InflictDamage;
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

        // FIX THIS
        // EnginKid Clustering Logic
        // Cluster spawning coroutine only starts if there are no clusters && enginKids are not in attack phase
        if (levelTimer > 1)
        {
            if (!enginKidClusterActive && !EnginKid.attackPhase)
            {
                EnginKidClusterCoroutine = StartCoroutine(SpawnEnginKidCluster());
            }
        }


        // Boss Spawns 1 time when levelTimer hits 180s
        if (levelTimer > 60 && !bossBattle)
        {
            bossBattle = true;
            cs1010.SetActive(true);
        }

        if (levelTimer > 100)
        {
            SceneManager.LoadScene("Cutscene3");
            //gameScreenManager.GoToLevel3();
        }

        levelTimer += Time.deltaTime;
    }

    // !!!Call this function when L2 final boss defeated, currently in CS1010Script when boss dies!!!
    public void LevelCompleted()
    {
        PowerUpManagerScript.Instance.levelThree = true;
        gameScreenManager.GoToLevel3();
    }

    // Damages
    void CleanerInflictDamage()
    {
        PiperScript.piperHealth -= 20;
    }
    void CSMuggerInflictDamage()
    {
        PiperScript.piperHealth -= 15;
    }
    void CSMuggerCodeInflictDamage()
    {
        PiperScript.piperHealth -= 5;
    }
    void CS1010InflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }
    // Spawn Enemies
    void SpawnCleaner()
    {
        ObjectPoolScript.spawnObject(cleaner, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void SpawnEnginKid()
    {
        ObjectPoolScript.spawnObject(enginKid, SpawnScript.generateSpawnPoint(), Quaternion.identity);
        EnginKid.enginKidCount += 1;
    }

    void StopEnginKidClusterCoroutine()
    {
        if (EnginKidClusterCoroutine != null)
        {
            StopCoroutine(EnginKidClusterCoroutine);
            EnginKidClusterCoroutine = null;
        }
    }
    void SpawnPaperBall()
    {
        // Spawn to the right of Piper
        ObjectPoolScript.spawnObject(paperBall, PiperScript.piperPosition + Vector3.right, Quaternion.identity);
    }
    void SpawnCSMugger()
    {
        ObjectPoolScript.spawnObject(csMugger, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }

    // Unsubscribe from Events
    private void OnDestroy()
    {
        CancelInvoke("spawnCSMugger");
        CancelInvoke("spawnCleaner");
        CSMugger.csMuggerCollisionEvent -= CSMuggerInflictDamage;
        Cleaner.cleanerCollisionEvent -= CleanerInflictDamage;
        EnginKid.enginKidDeathEvent -= StopEnginKidClusterCoroutine;
        CS1010ProjectileScript.CS1010ProjectileCollisionEvent -= CS1010InflictDamage;
    }

    private IEnumerator SpawnEnginKidCluster()
    {
        enginKidClusterActive = true;
        GameObject lastEnginKid;

        // Randomly generate a corner for the cluster to congregate at
        enginKidGatheringCorner = SpawnScript.generateEnginKidGatheringCorner();

        // Spawn an enginKid every 3s
        SpawnEnginKid();
        yield return new WaitForSeconds(secondsBetweenEnginKidSpawn);

        SpawnEnginKid();
        yield return new WaitForSeconds(secondsBetweenEnginKidSpawn);

        // Manually spawn an enginKid from pool because we want to
        // assign it to a gameObject to keep track of its distance to gathering point to trigger attackPhase
        lastEnginKid = ObjectPoolScript.spawnObject(enginKid, SpawnScript.generateSpawnPoint(), Quaternion.identity);
        EnginKid.enginKidCount += 1;

        // Wait for the last enginKid to gather
        while (lastEnginKid.activeInHierarchy)
        {
            // Activate attack phase when everybody has gathered
            if (Vector3.Distance(enginKidGatheringCorner, lastEnginKid.transform.position) < 0.6f)
            {
                EnginKid.attackPhase = true;
                // Activate group cluster boost
                EnginKid.gatheredSuccessfully = true;
                EnginKidGatheredEvent();
            }
            yield return null;
        }
        Debug.Log("End spawnEnginKidCluster Coroutine");
    }
}