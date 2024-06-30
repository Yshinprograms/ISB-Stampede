using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cleaner, EnginKid, CSMugger

public class L2LogicScript : MonoBehaviour
{
    private float timer = 0;

    // Import Game Manager Script & Game end conditions
    public GameScreenManager gameScreenManager;

    // Import GameObjects, drag and drop into Inspector
    public GameObject paperBall;
    public GameObject cleaner;
    public GameObject enginKid;
    public GameObject csMugger;
    public GameObject csMuggerCode;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenCleanerSpawn = 6f;
    public float secondsBetweenEnginKidSpawn = 5f;
    public float secondsBetweenCSMuggerSpawn = 10f;

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

        // Cleaner interaction and Spawns ; time 0s
        Cleaner.cleanerCollisionEvent += CleanerInflictDamage;
        InvokeRepeating(nameof(SpawnCleaner), 0f, secondsBetweenCleanerSpawn);

        // EnginKid interaction and Spawns
        EnginKid.enginKidDeathEvent += StopEnginKidClusterCoroutine;
        enginKidClusterActive = false;

        // CSMugger interaction and Spawns ; time 120s
        CSMugger.csMuggerCollisionEvent += CSMuggerInflictDamage;
        CSMuggerCode.csMuggerCodeCollisionEvent += CSMuggerCodeInflictDamage;
        InvokeRepeating(nameof(SpawnCSMugger), 120f, secondsBetweenCSMuggerSpawn);

        // Piper's parameters & projectile interactions
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

        // EnginKid Clustering Logic
        // Cluster spawning coroutine only starts if there are no clusters && enginKids are not in attack phase
        if (timer > 60)
        {
            if (!enginKidClusterActive && !EnginKid.attackPhase)
            {
                EnginKidClusterCoroutine = StartCoroutine(SpawnEnginKidCluster());
            }
        }

        if (timer > 2)
        {
            gameScreenManager.GoToLevel3();
        }

        timer += Time.deltaTime;
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
    }

    private IEnumerator SpawnEnginKidCluster()
    {
        enginKidClusterActive = true;
        GameObject lastEnginKid;

        // Randomly generate a corner for the cluster to congregate at
        enginKidGatheringCorner = SpawnScript.generateEnginKidGatheringCorner();

        // Spawn an enginKid every 3s
        SpawnEnginKid();
        yield return new WaitForSeconds(3);

        SpawnEnginKid();
        yield return new WaitForSeconds(3);

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
