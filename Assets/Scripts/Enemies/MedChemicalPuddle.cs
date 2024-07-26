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
    private float fadeSpeed;
    public Color puddleColor;
    Animator animPuddle;
    

    void OnEnable()
    {

        // Get all SpriteRenderer components in the children 
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            Color color = sr.color;
            color.a = 255;
            sr.color = color;
        }

        fadeDuration = 7;
        fadeSpeed = 1 / fadeDuration;
        timeInMap = 7;
        animPuddle = GetComponent<Animator>();
    }

    void Update()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        // Puddle fades
        if (timeInMap > 0)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                Color color = sr.color;
                color.a -= fadeSpeed * Time.deltaTime;
                color.a = Mathf.Clamp01(color.a);
                sr.color = color;
            }
        }
       
        // Puddle stays in map for 5s, after 5s puddle disappears
        timeInMap -= Time.deltaTime;
        if (timeInMap < 0)
        {
            ObjectPoolScript.returnObjectToPool(gameObject);
        }

        //animPuddle.Play("ChemicalPuddleIdle");

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
