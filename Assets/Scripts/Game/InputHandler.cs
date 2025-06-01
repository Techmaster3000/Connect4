using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private GameHandler gameHandler;
    private CursorHandler cursorHandler;
    [SerializeField] private PauseMenu pauseMenu;
    public bool aiTurn = false;

    public void Initialize(CursorHandler cursorHandler)
    {
        this.cursorHandler = cursorHandler;
        gameHandler = GetComponent<GameHandler>();
    }

    public void HandleInput(bool stopInput)
    {
        if (gameHandler.isBusy || aiTurn) return;
        HandlePauseInput();
        if (Time.timeScale == 0f || stopInput) return;

        HandleArrowInput();
        HandleSubmitInput();
    }

    private void HandleArrowInput()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cursorHandler.DecrementCursor();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cursorHandler.IncrementCursor();
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
    public void HandleAiSubmit()
    {
        Debug.Log("AI is making a move.");
        gameHandler.tryDropToken();
    }
}
