using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script consists of the functions used for the Game Over and Pause Script. 

public class GameScreenManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseMenu;

    // Game Over Screen will display 
    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void mainMenu()
    {
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

}
