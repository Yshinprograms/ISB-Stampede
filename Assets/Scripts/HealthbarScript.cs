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

    private void Start()
    {
        PiperScript.piperHealth = PiperScript.piperMaxHealth;
        healthbar.setMaxHealth(PiperScript.piperMaxHealth);
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
