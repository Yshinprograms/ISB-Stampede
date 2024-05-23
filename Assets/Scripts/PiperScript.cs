using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiperScript : MonoBehaviour
{
    public float piperMoveSpeed = 1f;
    public static Vector3 piperPosition;
    public static Collider2D enemyInRange;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        piperPosition = transform.position;

        //Find direction Piper needs to move based on WASD
        int dir = findDir(); 
        move(dir);

        // Checks if there are enemies in range of Piper's projectiles
        enemyInRange = Physics2D.OverlapCircle(transform.position, 2f, LayerMask.GetMask("Enemy"));
    }


    // Directions 0-8 indicate Not moving, N,NE,E,...,NW
    // Diagonals first, if not will always execute NSEW only
    private int findDir()
    {
        int dir = 0; //Not moving

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            dir = 2; //NE
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            dir = 4; //SE
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            dir = 6; //SW
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        {
            dir = 8; //NW
        }
        else if (Input.GetKey(KeyCode.W))
        {
            dir = 1; //N
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir = 3; //E
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir = 5; //S
        }
        else if (Input.GetKey(KeyCode.A))
        {
            dir = 7; //W
        }

        return dir;
    }

    private void move(int dir)
    {
        if (dir == 1) //N
        {
            transform.position += Vector3.up * piperMoveSpeed * Time.deltaTime;
        }
        if (dir == 2) //NE
        {
            transform.position += new Vector3(1, 1, 0).normalized * piperMoveSpeed * Time.deltaTime;
        }
        if (dir == 3) //E
        {
            transform.position += Vector3.right * piperMoveSpeed * Time.deltaTime;
        }
        if (dir == 4) //SE
        {
            transform.position += new Vector3(1, -1, 0).normalized * piperMoveSpeed * Time.deltaTime;
        }
        if (dir == 5) //S
        {
            transform.position += Vector3.down * piperMoveSpeed * Time.deltaTime;
        }
        if (dir == 6) //SW
        {
            transform.position += new Vector3(-1, -1, 0).normalized * piperMoveSpeed * Time.deltaTime;
        }
        if (dir == 7) //W
        {
            transform.position += Vector3.left * piperMoveSpeed * Time.deltaTime;
        }
        if (dir == 8) //NW
        {
            transform.position += new Vector3(-1, 1, 0).normalized * piperMoveSpeed * Time.deltaTime; 
        }
    }
}
