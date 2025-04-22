using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Volume GlobalVolume;
    private GameObject pauseScreen;

    void Start()
    {
        pauseScreen = GameObject.Find("PauseScreen");
        if (pauseScreen != null)
        {
            // Set the opacity to 0
            pauseScreen.GetComponent<CanvasGroup>().alpha = 0;
            pauseScreen.SetActive(false); // Deactivate the PauseScreen at the start
        }
        else
        {
            Debug.LogError("PauseScreen not found in the scene.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            TogglePauseMenu();
        }

    }
    public void TogglePauseMenu()
    {
        // Check if the game is currently paused
        UIHandler uiHandler = FindFirstObjectByType<UIHandler>();
        if (Time.timeScale == 0f)
        {
            // Resume the game
            Time.timeScale = 1f;
            //run animBlur from UIHandler script
            
            if (uiHandler != null)
            {
                StartCoroutine(uiHandler.animBlur());
            }
            else
            {
                Debug.LogError("UIHandler not found in the scene.");
            }
            pauseScreen.GetComponent<CanvasGroup>().alpha = 0; // Set the opacity to 0
            pauseScreen.SetActive(false); // Deactivate the PauseScreen
        }
        else
        {
            // Pause the game
            
            if (uiHandler != null)
            {
                StartCoroutine(uiHandler.animBlur());
            }
            else
            {
                Debug.LogError("UIHandler not found in the scene.");
            }
            pauseScreen.SetActive(true); // Activate the PauseScreen
            StartCoroutine(fadeUI());
            Time.timeScale = 0f;
        }
    }
    public IEnumerator fadeUI()
    {
        if (pauseScreen != null)
        {
            float start = pauseScreen.GetComponent<CanvasGroup>().alpha;
            float elapsed = 0f;
            float duration = 0.5f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsed / duration);
                pauseScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, 1, t);
                yield return null;
            }
        }
    }

}
