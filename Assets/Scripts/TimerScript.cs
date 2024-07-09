using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI sceneText;
    // public static float initialTime = 360;
    public static float remainingTime;

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

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);


        // Ensure the Text component is assigned
        if (sceneText == null)
        {
            Debug.LogError("SceneText is not assigned in the inspector!");
            return;
        }

        // Get the current scene name
        Scene currentScene = SceneManager.GetActiveScene();

        // Change the text based on the scene
        if (currentScene.name == "Level1")
        {
            sceneText.text = "LEVEL 1\n";
            sceneText.text = sceneText.text + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (currentScene.name == "Level2")
        {
            sceneText.text = "LEVEL 2\n";
            sceneText.text = sceneText.text + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (currentScene.name == "Level3")
        {
            sceneText.text = "LEVEL 3\n";
            sceneText.text = sceneText.text + string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
