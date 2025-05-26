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
    public Volume GlobalVolume;
    [SerializeField]
    public GameObject pauseScreen;
    [SerializeField]
    public GameObject settingsMenu;
    [SerializeField]
    public GameObject pauseMenu;

    void Start()
    {
        pauseScreen.GetComponent<CanvasGroup>().alpha = 0;
        pauseScreen.SetActive(false);
    }

    void Update()
    {
    }

    public void TogglePauseMenu()
    {
        UIHandler uiHandler = GlobalVolume.GetComponent<UIHandler>();

        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;

            if (uiHandler != null)
            {
                uiHandler.StopBlur();
            }
            else
            {
                Debug.LogError("UIHandler not found in the scene.");
            }

            pauseScreen.GetComponent<CanvasGroup>().alpha = 0;
            pauseScreen.SetActive(false);
            if (settingsMenu != null)
            {
                settingsMenu.SetActive(false);
            }
        }
        else
        {
            if (uiHandler != null)
            {
                uiHandler.StartBlur();
            }
            else
            {
                Debug.LogError("UIHandler not found in the scene.");
            }

            pauseScreen.SetActive(true);
            settingsMenu.SetActive(false);
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
