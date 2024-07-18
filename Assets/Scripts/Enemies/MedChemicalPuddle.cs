using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Poisonous chemical
- If Piper steps on Puddle, Piper loses 5 HP
- will stay in the map for 5s and disappear 
*/

public class MedChemicalPuddle : MonoBehaviour
{
    public delegate void StepOnPuddle();
    public static event StepOnPuddle StepOnPuddleEvent;
    private float timeInMap;

    private float fadeDuration;
    private SpriteRenderer sp;
    private float fadeSpeed;
    private Color color;

    void OnEnable()
    {
        sp = GetComponent<SpriteRenderer>();
        color = sp.color;
        color.a = 255;
        fadeDuration = 7;
        fadeSpeed = 1 / fadeDuration;
        timeInMap = 7;
    }

    /*void Start()
    {
        fadeDuration = 7;
        sp = GetComponent<SpriteRenderer>();
        fadeSpeed = 1 / fadeDuration;
        timeInMap = 7;
    }*/

    void Update()
    {
        // Puddle fades
        if (sp.color.a > 0)
        {
            //color = sp.color;
            color.a -= fadeSpeed * Time.deltaTime;
            color.a = Mathf.Clamp01(color.a); // Ensure alpha value is clamped between 0 and 1
            sp.color = color;
        }
       
        // Puddle stays in map for 5s, after 5s puddle disappears
        timeInMap -= Time.deltaTime;
        if (timeInMap < 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (StepOnPuddleEvent != null)
            {
                StepOnPuddleEvent();
            }
        }
    }

}
