using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalShot : MonoBehaviour
{
    private float maxSprayDist;
    Vector2 sprayPoint;
    private float spraySpeed;
    private float range = 0.5f;

    public GameObject medChemicalPuddle;

    void Start()
    {
        maxSprayDist = 4;
        spraySpeed = 4;
        range = 0.5f;
    }

    void OnEnable()
    {
        maxSprayDist = 4;
        spraySpeed = 4;
        range = 0.5f;
    }

    void Update()
    {
        //StartCoroutine(chemicalShotSequence());
        ChemicalSprayVector();
        transform.position = Vector2.MoveTowards(transform.position, sprayPoint, spraySpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, sprayPoint) < range)
        {
            ObjectPoolScript.spawnObject(medChemicalPuddle, transform.position, Quaternion.identity);
            // Destroy(gameObject);
            ObjectPoolScript.returnObjectToPool(gameObject);
        }
    }

    /*private IEnumerator chemicalShotSequence()
    {
        ChemicalSprayVector();
        transform.position = Vector2.MoveTowards(transform.position, sprayPoint, spraySpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, sprayPoint) < range)
        {
            ObjectPoolScript.spawnObject(medChemicalPuddle, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.5f);
        ObjectPoolScript.returnObjectToPool(gameObject);

    }*/

    void ChemicalSprayVector()
    {
        sprayPoint = new Vector2(Random.Range(-maxSprayDist, maxSprayDist), Random.Range(-maxSprayDist, maxSprayDist));
    }
}
