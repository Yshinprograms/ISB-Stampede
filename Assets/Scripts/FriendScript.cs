using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendScript : MonoBehaviour
{
    public GameObject paperBall;

    private float moveSpeed = 5f;
    private Vector3 targetPosition;
    public float secondsBetweenPaperBallSpawn;

    private float activeDuration;

    private void OnEnable()
    {
        secondsBetweenPaperBallSpawn = 0f;
        activeDuration = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = PiperScript.piperRealPosition + Vector3.up;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
        
        if (secondsBetweenPaperBallSpawn > 2.4f)
        {
            ObjectPoolScript.spawnObject(paperBall, transform.position + Vector3.right, paperBall.transform.rotation);
            secondsBetweenPaperBallSpawn = 0f;
        }

        if (activeDuration > 10f)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

        secondsBetweenPaperBallSpawn += Time.deltaTime;
        activeDuration += Time.deltaTime;
    }
}
