using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private GameHandler gameHandler;
    private CursorHandler cursorHandler;

    public void Initialize(GameHandler gameHandler, CursorHandler cursorHandler)
    {
        this.cursorHandler = cursorHandler;
        this.gameHandler = gameHandler;
    }

    public void HandleInput()
    {
        if (gameHandler.isBusy) return;

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

    private void HandleSubmitInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameHandler.tryDropToken();
        }
    }
}
