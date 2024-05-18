using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fix Bollard Parent starting position?

public class BollardScript : MonoBehaviour
{
    public delegate void Collision();
    public static event Collision bollardCollision;
    public float bollardSpeed = 0.7f;

    private Transform piperPosition;
    private AudioSource bollardCollisionSFX;

    void Start()
    {
        piperPosition = GameObject.FindGameObjectWithTag("Player").transform;
        bollardCollisionSFX = gameObject.GetComponent<AudioSource>();
    }


    void Update()
    {
        Vector3 directionToPiper = piperPosition.position - gameObject.transform.position; //Vector Addition
        moveToPiper(directionToPiper.normalized); //Normalize for constant speed in all directions
    }

    void moveToPiper(Vector3 directionToPiper)
    {
        gameObject.transform.position += directionToPiper * bollardSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (bollardCollision != null)
            {
                bollardCollision();
            }
            //Play sound effect here
            bollardCollisionSFX.Play();
        }
    }
}
