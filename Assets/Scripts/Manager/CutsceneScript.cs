using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CutsceneScript : MonoBehaviour
{
    public GameObject[] comicPanels; // Assign panel GameObjects in the Inspector
    private int currentPanelIndex = 0;

    public float fadeDuration = 1f;

    private bool isTransitioning = false; // Prevent multiple key presses during fade

    void Start()
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
    }

    void Update()
    {
        if (Input.anyKeyDown && !isTransitioning)
        {
            AdvanceToNextPanel();
        }
    }

    void AdvanceToNextPanel()
    {
        // Increment panel index, loop back to the beginning
        currentPanelIndex = (currentPanelIndex + 1) % comicPanels.Length;

        // Fade out the current panel (if any)
        StartCoroutine(FadePanelIn(comicPanels[currentPanelIndex]));
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
        TextMeshProUGUI panelText = panel.GetComponentInChildren<TextMeshProUGUI>();

        if (panelImage != null)
        {
            Color imageColor = panelImage.color;
            imageColor.a = alpha;
            panelImage.color = imageColor;
        }

        if (panelText != null)
        {
            Color textColor = panelText.color;
            textColor.a = alpha;
            panelText.color = textColor;
        }
    }

    // For possible future use
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
}