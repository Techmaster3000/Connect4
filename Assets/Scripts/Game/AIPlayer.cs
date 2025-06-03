using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    BoardManager boardManager;
    CursorHandler cursorHandler;
    InputHandler inputHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void initAI(BoardManager bm, CursorHandler ch, InputHandler ih) { 
        boardManager = bm;
        cursorHandler = ch;
        inputHandler = ih;


    }

    public void MakeMove()
    {
        if (!PlayWinningMove(2, (int)GetComponent<GameHandler>().gameMode))
        {
            return;
        
        }
        int column;
        while (true) { 
            column = Random.Range(0, boardManager.rowLength);
            if (boardManager.isColumnFull(column))
            {
                Debug.LogWarning("Column " + column + " is full. AI cannot make a move.");
            }
            else
            {
                break;
            }
        }

        moveCursor(column);
        inputHandler.HandleAiSubmit();
    }

    public bool PlayWinningMove(int aiPlayerNumber, int winLength)
    {
        // Try each column
        int[,] gridCopy = (int[,])boardManager.grid.Clone();
        GameObject[,] tokenGridCopy = (GameObject[,])boardManager.tokenGrid.Clone();
        for (int col = 0; col < boardManager.rowLength; col++)
        {
            if (boardManager.isColumnFull(col))
                continue;

            // Find the lowest empty row in this column
            int row = -1;
            for (int r = 0; r < boardManager.columnHeight; r++)
            {
                if (gridCopy[col, r] == 0)
                {
                    row = r;
                    break;
                }
            }
            if (row == -1) continue; // Should not happen

            // Simulate placing the AI's token
            gridCopy[col, row] = aiPlayerNumber;

            // Check for a win
            if (WinChecker.TryGetWinningTokens(
                gridCopy,
                tokenGridCopy,
                aiPlayerNumber,
                winLength,
                col,
                row,
                out var _))
            {

                // Move cursor and submit
                moveCursor(col);
                inputHandler.HandleAiSubmit();
                return false;
            }

            // Undo simulation
            boardManager.grid[col, row] = 0;
        }

        // If no winning move, fallback to random
        return true;
    }

    private void moveCursor(int targetColumn)
    {
        int moves = targetColumn - cursorHandler.cursorPosition;
        if (moves > 0)
        {
            for (int i = 0; i < moves; i++)
            {
                cursorHandler.IncrementCursor();
            }
        }
        else if (moves < 0)
        {
            for (int i = 0; i < -moves; i++)
            {
                cursorHandler.DecrementCursor();
            }
        }
    }
}
