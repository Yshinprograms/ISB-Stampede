using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

// Handles all UI, Spawning and logic in game
// Don't need static variables in here since the script is static

public class LogicScript : MonoBehaviour
{
    /*public int piperMaxHealth = 100;
    public int piperHealth; */
    public HealthbarScript healthbar;

    // Import Pause Menu Game Object and create boolean variable named GameIsPaused
    public GameObject pauseMenu;
    public static bool gameIsPaused = false;

    // Import Game Manager Script & Game end conditions
    public GameScreenManager gameScreenManager;
    private bool isAlive;

    // Import GameObjects, drag and drop into Inspector
    public GameObject bollard;
    public GameObject paperBall;
    public GameObject freshie;
    public GameObject csMugger;
    public GameObject csMuggerCode;
    public GameObject aunty;
    public GameObject cleaner;
    public GameObject enginKid;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenBollardSpawn = 4f;
    public float secondsBetweenFreshieSpawn = 6f;
    public float secondsBetweenAuntySpawn = 6f;
    public float secondsBetweenCleanerSpawn = 6f;
    public float secondsBetweenEnginKidSpawn = 5f;
    public float secondsBetweenCSMuggerSpawn = 10f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;

    // EnginKid Logic
    public bool enginKidClusterActive; // No longer static 
    public Vector3 enginKidGatheringCorner; // No longer static
    private Coroutine enginKidClusterCoroutine;
    public delegate void EnginEvent();
    public static event EnginEvent enginKidGatheredEvent;
    private float timer = 0;

    // Singleton Implementation
    private static LogicScript instance;
    public static LogicScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LogicScript>();
                if (instance == null)
                {
                    GameObject go = new GameObject("LogicManager");
                    instance = go.AddComponent<LogicScript>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        // Ensure only one instance exists
        /*if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);*/ 

        // Bollard interaction and Spawns ; time 0s
        Bollard.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating("spawnBollard", 0f, secondsBetweenBollardSpawn);

        // Freshie interaction and Spawns ; time 60s
        Freshie.freshieCollisionEvent += freshieInflictDamage;
        InvokeRepeating("spawnFreshie", 60f, secondsBetweenFreshieSpawn);

        // Aunty interactions and spawns ; time 120s
        Aunty.auntyCollisionEvent += auntyInflictDamage;
        Handbag.handbagCollisionEvent += handbagInflictDamage;
        InvokeRepeating("spawnAunty", 120f, secondsBetweenAuntySpawn);

        // Cleaner interaction and Spawns ; time 180s
        Cleaner.cleanerCollisionEvent += cleanerInflictDamage;
        InvokeRepeating("spawnCleaner", 180f, secondsBetweenCleanerSpawn);

        // EnginKid interaction and Spawns
        EnginKid.enginKidDeathEvent += stopEnginKidClusterCoroutine;
        enginKidClusterActive = false;

        // CSMugger interaction and Spawns ; time 300s
        CSMugger.csMuggerCollisionEvent += csMuggerInflictDamage;
        CSMuggerCode.csMuggerCodeCollisionEvent += csMuggerCodeInflictDamage;
        InvokeRepeating("spawnCSMugger", 300f, secondsBetweenCSMuggerSpawn);

        // Piper's parameters & projectile interactions
        isAlive = true;
        PiperScript.piperHealth = PiperScript.piperMaxHealth;
        healthbar.setMaxHealth(PiperScript.piperMaxHealth);
        PaperBallScript.activePaperBalls = 0;
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (gameIsPaused)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                gameIsPaused = false;

            } else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                gameIsPaused = true;
            }
        }

        // Set minimum health to 0
        healthbar.setHealth(PiperScript.piperHealth);

        if (PiperScript.piperHealth <= 0 && isAlive)
        {
            isAlive = false;
            gameScreenManager.gameOver();
            Debug.Log("Dead");
        }

        // Piper's projectile interactions
        secondsBetweenPaperBallSpawn += Time.deltaTime;
        if ((PaperBallScript.activePaperBalls < maxActivePaperBalls) && (secondsBetweenPaperBallSpawn > 1))
        {
            spawnPaperBall();
            secondsBetweenPaperBallSpawn = 0;
            PaperBallScript.activePaperBalls += 1;
        }

        // EnginKid Clustering Logic
        // Cluster spawning coroutine only starts if there are no clusters && enginKids are not in attack phase
        if (timer > 240)
        {
            if (!enginKidClusterActive && !EnginKid.attackPhase)
            {
                enginKidClusterCoroutine = StartCoroutine(spawnEnginKidCluster());
            }
        }
        if (timer > 150) // At 2min 30s stop wave 1
        {
            CancelInvoke("spawnBollard");
            CancelInvoke("spawnFreshie");
            CancelInvoke("spawnAunty");
        }

        // if timer more than 360s Game is completed  
        if (timer > 10)
        {
            gameScreenManager.GoToLevel2();
            //gameScreenManager.GameCompleted();
        }

        timer += Time.deltaTime;
    }

    // Damages
    void bollardInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }
    void freshieInflictDamage()
    {
        PiperScript.piperHealth -= 20;
    }
    void auntyInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }
    void cleanerInflictDamage()
    {
        PiperScript.piperHealth -= 20;
    }
    void handbagInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }

    void csMuggerInflictDamage()
    {
        PiperScript.piperHealth -= 15;
    }

    void csMuggerCodeInflictDamage()
    {
        PiperScript.piperHealth -= 5;
    }


    // Spawn Enemies
    void spawnBollard()
    {
        ObjectPoolScript.spawnObject(bollard, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void spawnFreshie()
    {
        ObjectPoolScript.spawnObject(freshie, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void spawnAunty()
    {
        ObjectPoolScript.spawnObject(aunty, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void spawnCleaner()
    {
        ObjectPoolScript.spawnObject(cleaner, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }
    void spawnEnginKid()
    {
        ObjectPoolScript.spawnObject(enginKid, SpawnScript.generateSpawnPoint(), Quaternion.identity);
        EnginKid.enginKidCount += 1;
    }
    void stopEnginKidClusterCoroutine()
    {
        if (enginKidClusterCoroutine != null)
        {
            StopCoroutine(enginKidClusterCoroutine);
            enginKidClusterCoroutine = null;
        }
    }

    void spawnCSMugger()
    {
        ObjectPoolScript.spawnObject(csMugger, SpawnScript.generateSpawnPoint(), Quaternion.identity);
    }

    // Spawn Projectiles
    void spawnPaperBall()
    {
        // Spawn to the right of Piper
        ObjectPoolScript.spawnObject(paperBall, PiperScript.piperPosition + Vector3.right, Quaternion.identity);
    }

    // Unsubscribe from Events
    private void OnDestroy()
    {
        CancelInvoke("spawnBollard");
        CancelInvoke("spawnFreshie");
        CancelInvoke("spawnCSMugger");
        CancelInvoke("spawnAunty");
        CancelInvoke("spawnCleaner");
        Bollard.bollardCollisionEvent -= bollardInflictDamage;
        Freshie.freshieCollisionEvent -= freshieInflictDamage;
        CSMugger.csMuggerCollisionEvent -= csMuggerInflictDamage;
        Aunty.auntyCollisionEvent -= auntyInflictDamage;
        Handbag.handbagCollisionEvent -= handbagInflictDamage;
        Cleaner.cleanerCollisionEvent -= cleanerInflictDamage;
        EnginKid.enginKidDeathEvent -= stopEnginKidClusterCoroutine;
    }

    // Coroutine to spawn 3 EnginKids every 3 seconds
    private IEnumerator spawnEnginKidCluster()
    {
        Debug.Log("Start spawnEnginKidCluster Coroutine");
        enginKidClusterActive = true;
        GameObject lastEnginKid;

        // Randomly generate a corner for the cluster to congregate at
        enginKidGatheringCorner = SpawnScript.generateEnginKidGatheringCorner();

        // Spawn an enginKid every 3s
        spawnEnginKid();
        yield return new WaitForSeconds(3);

        spawnEnginKid();
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
                enginKidGatheredEvent();
            }
            yield return null;
        }
        Debug.Log("End spawnEnginKidCluster Coroutine");
    }
}
