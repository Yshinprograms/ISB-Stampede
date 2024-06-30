using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalaScript : MonoBehaviour
{
    private float activeDuration;
    public float timeOnMap = 6f;

    private void OnEnable()
    {
        activeDuration = 0f;
    }
    void Update()
    {
        PiperScript.piperPosition = transform.position;

        if (activeDuration > timeOnMap)
        {
            PiperScript.malaActive = false;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

        activeDuration += Time.deltaTime;
    }
}
