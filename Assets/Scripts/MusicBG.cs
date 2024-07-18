using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicBG : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    public AudioSource mainMenuBG;
    public AudioSource cutsceneBG;
    public AudioSource level1BG;
    public AudioSource level2BG;
    public AudioSource level3BG;


    // Get the current scene's name
    //string currentSceneName;

    void Start()
    {
        //currentSceneName = SceneManager.GetActiveScene().name;
        //mainMenuBG.Play();

    }



    /*public void mainMenuBGMusic()
    {
        mainMenuBG.Play();
    }

    void cutsceneBGMusic()
    {
        cutsceneBG.Play();
    }

    void level1BGMusic()
    {
        level1BG.Play();
    }

    void level2BGMusic()
    {
        level2BG.Play();
    }
    void level3BGMusic()
    {
        level3BG.Play();
    }*/

    // Update is called once per frame
    /*void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "MainMenu")
        {
            mainMenuBG.Play();
        }

        if (currentSceneName == "Cutscene1")
        {
           mainMenuBG.Pause();
           cutsceneBG.Play();
        }

        if (currentSceneName == "Level1")
        {
            cutsceneBG.Pause();
            level1BG.Play();
        }

        if (currentSceneName == "Cutscene2")
        {
            level1BG.Pause();
            cutsceneBG.Play();
        }

        if (currentSceneName == "Level2")
        {
            cutsceneBG.Pause();
            level2BG.Play();
        }

        if (currentSceneName == "Cutscene3")
        {
            level2BG.Pause();
            cutsceneBG.Play();
        }

        if (currentSceneName == "Level3")
        {
            cutsceneBG.Pause();
            level3BG.Play();
        }
    }*/

}
