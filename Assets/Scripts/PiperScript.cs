using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiperScript : MonoBehaviour
{
    // Piper's projectile range is 2f
    // Health settings
    public static int piperMaxHealth = 100;
    public static int piperHealth;
    public static int allEnemyMask;
    public static bool malaActive = false;

    public static float piperMoveSpeed = 5f;
    public static Vector3 piperPosition;
    public static Vector3 piperRealPosition;
    public static Collider2D enemyInRange;
    private SpriteRenderer sp;
    private float piperStunTimer = 0f;
    Animator anim;


    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        ChineseTourist.PhotoEvent += PiperStunned;
        
        // Add future enemy layers here as needed, bitmasking; Note the int size in C#(32 bits / Max layers)
        // physics2d takes layermask as argument
        allEnemyMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("ChineseTourist");
    }

    // Update is called once per frame
    void Update()
    {
        piperRealPosition = transform.position;
        //Find direction Piper needs to move based on WASD
        int dir = findDir();

        // Piper is unable to move for 0.5s after getting stunned by ChineseTourist
        if (piperStunTimer > 0.6f)
        {
            Move(dir);
        }

        if (!malaActive)
        {
            piperPosition = transform.position;
        }


        // Checks if there are enemies in range of Piper's projectiles
        enemyInRange = Physics2D.OverlapCircle(transform.position, 2f, allEnemyMask);

        if (Input.GetKeyDown(KeyCode.E))
        {
            PiperPush();
        }

        // Make sure Piper remains upright after colliding into other objects
        transform.rotation = Quaternion.Euler(0, 0, 0);

        piperStunTimer += Time.deltaTime;
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
            //sp.flipX = true;   

        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            dir = 6; //SW
            //sp.flipX = false;
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
            //sp.flipX = true;
       
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir = 5; //S
        }
        else if (Input.GetKey(KeyCode.A))
        {
            dir = 7; //W
            //sp.flipX = false;
        }

        return dir;
    }

    private void Move(int dir)
    {
        if (dir == 1) //N
        {
            transform.position += Vector3.up * piperMoveSpeed * Time.deltaTime;
            transform.localScale = new Vector2(0.15f, 0.15f);
            anim.Play("PiperMoving");
        }
        if (dir == 2) //NE
        {
            transform.position += new Vector3(1, 1, 0).normalized * piperMoveSpeed * Time.deltaTime;
            transform.localScale = new Vector2(0.15f, 0.15f);
            anim.Play("PiperMoving");
        }
        if (dir == 3) //E
        {
            transform.position += Vector3.right * piperMoveSpeed * Time.deltaTime;
            transform.localScale = new Vector2(0.15f, 0.15f);
            anim.Play("PiperMoving");
        }
        if (dir == 4) //SE
        {
            transform.position += new Vector3(1, -1, 0).normalized * piperMoveSpeed * Time.deltaTime;
            transform.localScale = new Vector2(0.15f, 0.15f);
            anim.Play("PiperMoving");
        }
        if (dir == 5) //S
        {
            transform.position += Vector3.down * piperMoveSpeed * Time.deltaTime;
            transform.localScale = new Vector2(0.15f, 0.15f);
            anim.Play("PiperMoving");
        }
        if (dir == 6) //SW
        {
            transform.position += new Vector3(-1, -1, 0).normalized * piperMoveSpeed * Time.deltaTime;
            transform.localScale = new Vector2(-0.15f, 0.15f);
            anim.Play("PiperMoving");

        }
        if (dir == 7) //W
        {
            transform.position += Vector3.left * piperMoveSpeed * Time.deltaTime;
            transform.localScale = new Vector2(-0.15f, 0.15f);
            anim.Play("PiperMoving");

        }
        if (dir == 8) //NW
        {
            transform.position += new Vector3(-1, 1, 0).normalized * piperMoveSpeed * Time.deltaTime;
            transform.localScale = new Vector2(-0.15f, 0.15f);
            anim.Play("PiperMoving");

        }
        if (dir == 0) //Not moving
        {
            anim.Play("PiperIdle");
        }
    }

    // Function to stun Piper by Chinese Tourists. Didn't unsubscribe onDestroy because gameOver when Piper gets destroyed
    void PiperStunned()
    {
        piperStunTimer = 0f;
    }

    public static void ApplyHealthBuff(int amount)
    {
        piperHealth = Mathf.Clamp(piperHealth + amount, 0, piperMaxHealth);
    }

    // Trigger this upon 'e' for Piper to push away InnocentStudents!
    void PiperPush()
    {
        Collider2D[] innocentStudentsInRange = Physics2D.OverlapCircleAll(transform.position, 2f, LayerMask.GetMask("InnocentStudent"));

        // Loop through the students
        foreach (Collider2D student in innocentStudentsInRange)
        {
            if (Vector2.Distance(student.transform.position, piperPosition) < 3f)
            {
                InnocentStudentScript IS = student.GetComponent<InnocentStudentScript>();
                IS.PushBack();
                Debug.Log("Pushback");
            }
        }
    }

}
