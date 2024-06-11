using UnityEngine;

public class BizSnakeProjectileScript : MonoBehaviour
{
    public delegate void BizSnakeProjectileEvent();
    public static event BizSnakeProjectileEvent BizSnakeProjectileCollisionEvent;

    private readonly float speed = 4f;
    private float timeActive;
    private float delayTimeBetweenSpawnAndThrown = 0;
    private Vector3 directionToPiper = Vector3.zero;

    void Start()
    {
        BizSnakeProjectileCollisionEvent += BizInflictDamage;
    }
    private void OnEnable()
    {
        timeActive = 0;
        delayTimeBetweenSpawnAndThrown = 0;
        directionToPiper = Vector3.zero;
    }

    void Update()
    {
        // Get directionToPiper only once after 1s
        if (delayTimeBetweenSpawnAndThrown > 1 && directionToPiper == Vector3.zero)
        {
            directionToPiper = (PiperScript.piperPosition - transform.position).normalized;
        }

        // Normalize for constant speed in all directions
        transform.position += speed * Time.deltaTime * directionToPiper;

        // Deactivate if no collision after 10s
        if (timeActive > 10)
        {
            timeActive = 0;
            delayTimeBetweenSpawnAndThrown = 0;
            directionToPiper = Vector3.zero;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

        timeActive += Time.deltaTime;
        delayTimeBetweenSpawnAndThrown += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BizSnakeProjectileCollisionEvent?.Invoke();
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }

    void BizInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }

    private void OnDestroy()
    {
        BizSnakeProjectileCollisionEvent -= BizInflictDamage;
    }
}
