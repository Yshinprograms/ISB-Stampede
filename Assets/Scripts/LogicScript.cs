using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int piperHealth = 100;
    public Text healthbar;

    // Start is called before the first frame update
    void Start()
    {
        BollardScript.bollardCollision += bollardInflictDamage;
        
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.text = piperHealth.ToString();
    }

    void bollardInflictDamage()
    {
        piperHealth -= 10;
    }

    private void OnDestroy()
    {
        BollardScript.bollardCollision -= bollardInflictDamage;
    }
}
