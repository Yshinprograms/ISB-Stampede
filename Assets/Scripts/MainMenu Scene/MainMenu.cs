using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void playGame()
    {
        //Destroy(Master);
        SceneManager.LoadScene("Level1");
        //piper.transform.position = Vector3.zero;
        TimerScript.remainingTime = L1LogicScript.levelOneDuration;
        //Time.timeScale = 1;
        //pauseMenu.SetActive(false);
        //gameOverUI.SetActive(false);
        Debug.Log("Play game");

    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }
}
