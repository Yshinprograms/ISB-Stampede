using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbarScript : MonoBehaviour
{
    public Slider slider;
    public BossHealthbarScript healthbar;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void EnableHealthbar()
    {
        gameObject.SetActive(true);
    }

    public void DisableHealthbar()
    {
        gameObject.SetActive(false);
    }
}