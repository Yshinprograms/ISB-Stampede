using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script consists of the functions used for the Game Over and Pause Script. 

public class GameScreenManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseMenu;
    public GameObject gameCompletedUI;
    public GameObject piper;

    public static bool gameIsPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                gameIsPaused = false;

            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                gameIsPaused = true;
            }
        }
    }

    // Game Over Screen will display 
    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void restartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Level1");
        PiperScript.piperHealth = PiperScript.piperMaxHealth;
        piper.transform.position = Vector3.zero;
        TimerScript.remainingTime = TimerScript.initialTime;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        gameOverUI.SetActive(false);
    }

    public void GoToLevel2()
    {
        SceneManager.LoadScene("Level2");
        Time.timeScale = 1;
    }

    public void GoToLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void mainMenu()
    {
        PiperScript.piperHealth = PiperScript.piperMaxHealth;
        piper.transform.position = Vector3.zero;
        gameOverUI.SetActive(false);
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;   
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void pauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameCompleted()
    {
        gameCompletedUI.SetActive(true);
        Time.timeScale = 0;
    }

}
