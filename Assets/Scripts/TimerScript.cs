using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;
    public static float initialTime = 360;
    public static float remainingTime = initialTime;
    //public Text sceneText;

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        } else
        {
            remainingTime = 0;
        }
        //int minutes = Mathf.FloorToInt(remainingTime / 60);
        //int seconds = Mathf.FloorToInt(remainingTime % 60);

        //timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Ensure the Text component is assigned
        if (timerText == null)
        {
            Debug.LogError("SceneText is not assigned in the inspector!");
            return;
        }

        // Get the current scene name
        Scene currentScene = SceneManager.GetActiveScene();

        // Change the text based on the scene
        if (currentScene.name == "Level1")
        {
            timerText.text = "LEVEL 1";
        }
        else if (currentScene.name == "Level2")
        {
            timerText.text = "LEVEL 2";
        }
        else if (currentScene.name == "Level3")
        {
            timerText.text = "LEVEL 3";
        }
    }
}
