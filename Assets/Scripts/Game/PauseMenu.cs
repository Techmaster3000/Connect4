using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

/// <summary>
/// Manages the pause menu functionality, including pausing and resuming the game,
/// as well as handling UI transitions and effects.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// The global volume used for rendering effects like blur.
    /// </summary>
    public Volume GlobalVolume;

    /// <summary>
    /// Reference to the pause screen GameObject.
    /// </summary>
    private GameObject pauseScreen;

    /// <summary>
    /// Initializes the pause menu by finding the pause screen in the scene
    /// and setting its initial state to inactive.
    /// </summary>
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

    /// <summary>
    /// Monitors input for toggling the pause menu and checks if input is disabled.
    /// </summary>
    void Update()
    {
        // Check if stopInput is true
        GameObject gameHandler = GameObject.Find("GameHandler");
        if (gameHandler != null)
        {
            GameHandler gameHandlerScript = gameHandler.GetComponent<GameHandler>();
            if (gameHandlerScript != null && gameHandlerScript.stopInput)
            {
                return; // Skip the rest of the Update method
            }
        }

        // Toggle the pause menu when Escape or Backspace is pressed
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            TogglePauseMenu();
        }
    }

    /// <summary>
    /// Toggles the pause menu on or off, pausing or resuming the game accordingly.
    /// </summary>
    public void TogglePauseMenu()
    {
        // Check if the game is currently paused
        UIHandler uiHandler = FindFirstObjectByType<UIHandler>();
        if (Time.timeScale == 0f)
        {
            // Resume the game
            Time.timeScale = 1f;

            if (uiHandler != null)
            {
                uiHandler.StopBlur(); // Unblur when resuming the game
            }
            else
            {
                Debug.LogError("UIHandler not found in the scene.");
            }

            pauseScreen.GetComponent<CanvasGroup>().alpha = 0;
            pauseScreen.SetActive(false); // Deactivate the PauseScreen
        }
        else
        {
            // Pause the game
            if (uiHandler != null)
            {
                uiHandler.StartBlur();
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

    /// <summary>
    /// Fades in the pause screen UI over a short duration.
    /// </summary>
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
