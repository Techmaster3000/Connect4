using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private GameHandler gameHandler;

    public void Initialize(GameHandler handler)
    {
        gameHandler = handler;
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
            gameHandler.DecrementCursor();
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameHandler.IncrementCursor();
            moved = true;
        }

        if (moved)
        {
            gameHandler.UpdateCursorPosition();
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
