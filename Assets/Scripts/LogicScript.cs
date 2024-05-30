using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

// Handles all UI, Spawning and logic in game
// Use a coroutine to handle spawning of enginKid every 3 seconds --> cluster attacks in 9s
// Activate coroutine when no of enginKids == 0

public class LogicScript : MonoBehaviour
{
    public int piperMaxHealth = 100;
    public static int piperHealth;
    public HealthbarScript healthbar;

    // Import Pause Menu Game Object and create boolean variable named GameIsPaused
    public GameObject pauseMenu;
    public static bool GameIsPaused = false;

    // Import Game Manager Script & Game end conditions
    public GameManagerScript gameManager;
    private bool isAlive;

    // Import GameObjects, drag and drop into Inspector
    public GameObject bollard;
    public GameObject paperBall;
    public GameObject freshie;
    public GameObject csMugger;
    public GameObject csMuggerCodeSpawn;
    public GameObject aunty;
    public GameObject cleaner;
    public GameObject enginKid;

    // Spawn times
    public float secondsBetweenPaperBallSpawn;
    public float secondsBetweenBollardSpawn = 6f;
    public float secondsBetweenFreshieSpawn = 5f;
    public float secondsBetweenCSMuggerSpawn = 6f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;
    
    public float secondsBetweenFreshieSpawn = 6f;
    public float secondsBetweenAuntySpawn = 6f;
    public float secondsBetweenCleanerSpawn = 5f;
    public float secondsBetweenEnginKidSpawn = 5f;

    // Controls quantity of projectiles on map
    public int maxActivePaperBalls = 1;

    // EnginKid Logic
    public static bool enginKidClusterActive;
    public static Vector3 enginKidGatheringCorner;
    private Coroutine enginKidClusterCoroutine;
    public delegate void EnginEvent();
    public static event EnginEvent enginKidGatheredEvent;

    void Start()
    {

        // Freshie interaction and Spawns 
        FreshieScript.freshieCollisionEvent += freshieInflictDamage;
        InvokeRepeating("spawnFreshie", 0f, secondsBetweenFreshieSpawn); // Calls freshieBollard every 5s from t=0

        // CSMugger interaction and Spawns + Future enemies
        CSMuggerScript.csMuggerCollisionEvent += csMuggerInflictDamage;
        InvokeRepeating("spawnCSMugger", 0f, secondsBetweenCSMuggerSpawn);
        csMuggerCodeSpawnScript.csMuggerCodeCollisionEvent += csMuggerCodeInflictDamage;

        // Bollard interaction and Spawns
        BollardScript.bollardCollisionEvent += bollardInflictDamage;
        InvokeRepeating("spawnBollard", 1110f, secondsBetweenBollardSpawn);

        // Freshie interaction and Spawns
        FreshieScript.freshieCollisionEvent += freshieInflictDamage;
        InvokeRepeating("spawnFreshie", 1110f, secondsBetweenFreshieSpawn);

        // Aunty interactions and spawns
        AuntyScript.auntyCollisionEvent += auntyInflictDamage;
        HandbagScript.handbagCollisionEvent += handbagInflictDamage;
        InvokeRepeating("spawnAunty", 1110f, secondsBetweenAuntySpawn);

        // Cleaner interaction and Spawns
        CleanerScript.cleanerCollisionEvent += cleanerInflictDamage;
        InvokeRepeating("spawnCleaner", 1110f, secondsBetweenFreshieSpawn);

        // EnginKid interaction and Spawns
        EnginKidScript.enginKidDeathEvent += stopEnginKidClusterCoroutine;
        enginKidClusterActive = false;

        // Piper's parameters & projectile interactions
        isAlive = true;
        piperHealth = piperMaxHealth;
        healthbar.setMaxHealth(piperMaxHealth);
        PaperBallScript.activePaperBalls = 0;


    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (GameIsPaused)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                GameIsPaused = false;

            } else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                GameIsPaused = true;
            }
        }

        // Set minimum health to 0
        healthbar.setHealth(piperHealth);

        if (piperHealth <= 0 && isAlive)
        {
            isAlive = false;
            gameManager.gameOver();
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
        if (!enginKidClusterActive && !EnginKidScript.attackPhase)
        {
            enginKidClusterCoroutine = StartCoroutine(spawnEnginKidCluster());
        }

        //Debug.Log(EnginKidScript.enginKidCount.ToString());
    }

    // Damages
    void bollardInflictDamage()
    {
        piperHealth -= 10;
    }
    void freshieInflictDamage()
    {
        piperHealth -= 20;
    }
    void auntyInflictDamage()
    {
        piperHealth -= 10;
    }
    void cleanerInflictDamage()
    {
        piperHealth -= 20;
    }
    void handbagInflictDamage()
    {
        piperHealth -= 10;
    }

    void csMuggerInflictDamage()
    {
        piperHealth -= 15;
    }

    void csMuggerCodeInflictDamage()
    {
        piperHealth -= 5;
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
        EnginKidScript.enginKidCount += 1;
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
        BollardScript.bollardCollisionEvent -= bollardInflictDamage;
        FreshieScript.freshieCollisionEvent -= freshieInflictDamage;
        CSMuggerScript.csMuggerCollisionEvent -= csMuggerInflictDamage;
        AuntyScript.auntyCollisionEvent -= auntyInflictDamage;
        HandbagScript.handbagCollisionEvent -= handbagInflictDamage;
        CleanerScript.cleanerCollisionEvent -= cleanerInflictDamage;
        EnginKidScript.enginKidDeathEvent -= stopEnginKidClusterCoroutine;
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
        EnginKidScript.enginKidCount += 1;
        
        // Wait for the last enginKid to gather
        while (lastEnginKid.activeInHierarchy)
        {
            // Activate attack phase when everybody has gathered
            if (Vector3.Distance(enginKidGatheringCorner, lastEnginKid.transform.position) < 0.6f)
            {
                EnginKidScript.attackPhase = true;
                // Activate group cluster boost
                EnginKidScript.gatheredSuccessfully = true;
                enginKidGatheredEvent();
            }
            yield return null;
        }
        Debug.Log("End spawnEnginKidCluster Coroutine");
    }
}
