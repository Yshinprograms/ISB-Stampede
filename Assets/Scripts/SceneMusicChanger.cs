using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicChanger : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
        // Plays background Music
        musicSource.Play();
    }

}
