using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void playGame()
    {

        // SceneManager.LoadScene("Cutscene1");
        SceneManager.LoadScene("Level2");
        TimerScript.remainingTime = L1LogicScript.levelOneDuration;
        Debug.Log("Play game");

    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}
