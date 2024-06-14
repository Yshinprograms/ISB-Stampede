using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalaScript : MonoBehaviour
{
    private float activeDuration;

    private void OnEnable()
    {
        activeDuration = 0f;
    }
    void Update()
    {
        PiperScript.piperPosition = transform.position;

        if (activeDuration > 6f)
        {
            PiperScript.malaActive = false;
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

        activeDuration += Time.deltaTime;
    }
}
