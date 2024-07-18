using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneScript : MonoBehaviour
{
    public GameObject[] comicPanels; // Assign panel GameObjects in the Inspector
    public AudioSource beep;
    private int currentPanelIndex = 0;
    private int panelCount;

    public float fadeDuration = 1f;

    private bool isTransitioning = false; // Prevent multiple key presses during fade

    // Get the current scene's name
    string currentSceneName;

    /*void Start()
    {
        // Initially hide all panels 
        foreach (GameObject panel in comicPanels)
        {
            SetPanelOpacity(panel, 0f);
        }

        // Fade in the first panel at the start
        if (comicPanels.Length > 0)
        {
            StartCoroutine(FadePanelIn(comicPanels[currentPanelIndex]));
        }
    }*/

    private void OnEnable()
    {
        panelCount = 0;

        // Get the current scene's name
        currentSceneName = SceneManager.GetActiveScene().name;

        // Initially hide all panels 
        foreach (GameObject panel in comicPanels)
        {
            SetPanelOpacity(panel, 0f);
        }

        // Fade in the first panel at the start
        if (comicPanels.Length > 0)
        {
            StartCoroutine(FadePanelIn(comicPanels[currentPanelIndex]));
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && !isTransitioning)
        {
            AdvanceToNextPanel();
            beep.Play();
            panelCount += 1;
            
        }
        if (panelCount >= comicPanels.Length)
        {
            if (currentSceneName == "Cutscene1")
            {
                SceneManager.LoadScene("Level1");
          
            }

            if (currentSceneName == "Cutscene2")
            {
                SceneManager.LoadScene("Level2");
               
            }

            if (currentSceneName == "Cutscene3")
            {
                SceneManager.LoadScene("Level3");
              
            }


        }

    }

    void AdvanceToNextPanel()
    {
        // Increment panel index, loop back to the beginning
        // currentPanelIndex = (currentPanelIndex + 1) % comicPanels.Length;

        if (currentPanelIndex < comicPanels.Length -1)
        {
            currentPanelIndex = currentPanelIndex + 1;
            StartCoroutine(FadePanelIn(comicPanels[currentPanelIndex]));
        }
        
    }

    IEnumerator FadePanelIn(GameObject panel)
    {
        isTransitioning = true;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            SetPanelOpacity(panel, Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SetPanelOpacity(panel, 1f);
        isTransitioning = false;
    }

    void SetPanelOpacity(GameObject panel, float alpha)
    {
        Image panelImage = panel.GetComponent<Image>();
        TextMeshProUGUI[] panelTexts = panel.GetComponentsInChildren<TextMeshProUGUI>();

        if (panelImage != null)
        {
            Color imageColor = panelImage.color;
            imageColor.a = alpha;
            panelImage.color = imageColor;
        }

        foreach (TextMeshProUGUI panelText in panelTexts)
        {
            Color textColor = panelText.color;
            textColor.a = alpha;
            panelText.color = textColor;
        }
    }

    // For possible future use
    /*
    IEnumerator FadePanelOut(GameObject panel)
    {
        isTransitioning = true;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            SetPanelOpacity(panel, Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SetPanelOpacity(panel, 0f);
        StartCoroutine(FadePanelIn(comicPanels[currentPanelIndex])); // Fade in the next panel
    }
    */
}