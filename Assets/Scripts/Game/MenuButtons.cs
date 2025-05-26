using UnityEngine;

public class MenuButtons: MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RestartGame()
    {
        // Reload the current scene to restart the game
        Time.timeScale = 1f; // Ensure time scale is reset
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenSettingsMenu()
    {
        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null)
        {
            // Hide and deactivate the pause screen
            if (pauseMenu.pauseMenu != null)
            {
                pauseMenu.pauseMenu.SetActive(false);
            }

            // Show the settings menu
            if (pauseMenu.settingsMenu != null)
            {
                pauseMenu.settingsMenu.SetActive(true);
                // Optionally, reset CanvasGroup alpha if needed
                var cg = pauseMenu.settingsMenu.GetComponent<CanvasGroup>();
                if (cg != null) cg.alpha = 1;
            }
            else
            {
                Debug.LogError("settingsMenu is not assigned in PauseMenu.");
            }
        }
        else
        {
            Debug.LogError("PauseMenu instance not found in the scene.");
        }
    }

    public void ToMenu()
    {
        // Load the main menu scene
        Time.timeScale = 1f; // Ensure time scale is reset
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
