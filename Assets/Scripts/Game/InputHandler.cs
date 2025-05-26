using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private GameHandler gameHandler;
    private CursorHandler cursorHandler;
    [SerializeField] private PauseMenu pauseMenu;

    public void Initialize(GameHandler gameHandler, CursorHandler cursorHandler)
    {
        this.cursorHandler = cursorHandler;
        this.gameHandler = gameHandler;
    }

    public void HandleInput(bool stopInput)
    {
        if (gameHandler.isBusy) return;
        HandlePauseInput();
        if (Time.timeScale == 0f || stopInput) return;

        HandleArrowInput();
        HandleSubmitInput();
        
    }

    private void HandleArrowInput()
    {
        bool moved = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cursorHandler.DecrementCursor();
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cursorHandler.IncrementCursor();
            moved = true;
        }

        if (moved)
        {
            cursorHandler.UpdateCursorPosition();
        }
    }
    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            if (pauseMenu != null)
            {
                pauseMenu.TogglePauseMenu();
            }
        }
    }

    private void HandleSubmitInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameHandler.tryDropToken();
        }
    }
}
