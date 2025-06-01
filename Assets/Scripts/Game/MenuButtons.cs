using UnityEngine;

public class MenuButtons: MonoBehaviour
{
    PauseMenu pauseMenu;
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
    public void CloseSettingsMenu()
    {
        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null)
        {
            // Hide and deactivate the settings menu
            if (pauseMenu.settingsMenu != null)
            {
                pauseMenu.settingsMenu.SetActive(false);
                // Optionally, reset CanvasGroup alpha if needed
            }
            else
            {
                Debug.LogError("settingsMenu is not assigned in PauseMenu.");
            }
            // Show the pause screen again
            if (pauseMenu.pauseMenu != null)
            {
                pauseMenu.pauseMenu.SetActive(true);
                var cg = pauseMenu.pauseScreen.GetComponent<CanvasGroup>();
                if (cg != null) cg.alpha = 1;
            }
        }
        else
        {
            Debug.LogError("PauseMenu instance not found in the scene.");
        }
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
                //run the LoadFpsLimit function from SettingsMenu
                SettingsMenu settingsMenu = pauseMenu.settingsMenu.GetComponent<SettingsMenu>();
                if (settingsMenu != null)
                {
                    settingsMenu.loadFpsLimit(); // Ensure this method exists in SettingsMenu
                }
                else
                {
                    Debug.LogError("SettingsMenu component not found on settingsMenu GameObject.");
                }
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
    public void ResumeGame() {
        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        pauseMenu.TogglePauseMenu();

    }

    public void ToMenu()
    {
        // Load the main menu scene
        Time.timeScale = 1f; // Ensure time scale is reset
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
