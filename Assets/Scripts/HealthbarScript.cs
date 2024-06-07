using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    public Slider slider;
    public HealthbarScript healthbar;

    // Import Game Manager Script & Game end conditions
    public GameScreenManager gameScreenManager;

    void Awake()
    {
        // Piper's parameters & projectile interactions
        PiperScript.piperHealth = PiperScript.piperMaxHealth;
        healthbar.setMaxHealth(PiperScript.piperMaxHealth);
        PaperBallScript.activePaperBalls = 0;
    }

    void Update()
    {
        // Set minimum health to 0
        healthbar.setHealth(PiperScript.piperHealth);

        if (PiperScript.piperHealth <= 0)
        {
            gameScreenManager.gameOver();
            Debug.Log("Dead");
        }
    }

    // Damages
    /* void bollardInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }
    void freshieInflictDamage()
    {
        PiperScript.piperHealth -= 20;
    }
    void auntyInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }
    void cleanerInflictDamage()
    {
        PiperScript.piperHealth -= 20;
    }
    void handbagInflictDamage()
    {
        PiperScript.piperHealth -= 10;
    }

    void csMuggerInflictDamage()
    {
        PiperScript.piperHealth -= 15;
    }

    void csMuggerCodeInflictDamage()
    {
        PiperScript.piperHealth -= 5;
    } */

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void setHealth(int health)
    {
        slider.value = health;
    }
}
