using System;
using UnityEngine;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    public int columnHeight;
    public int rowLength;
    public int[] fullColumns;
    public int[,] grid;
    public GameObject[,] tokenGrid;

    public void markColumnFull(int colNum)
    {
        fullColumns[colNum] = 1;

    }
    public bool isColumnFull(int cursorPosition)
    {
        if (fullColumns[cursorPosition] == 1)
        {

            return true;
        }
        return false;
    }
    public bool isBoardFull()
    {
        if (Array.TrueForAll(fullColumns, x => x == 1))
        {

            return true;
        }
        return false;
    }

    public (Vector3, int, int) getlandPos(int currentPlayer, int cursorPosition)
    {
        if (isColumnFull(cursorPosition))
        {
            throw new System.Exception("Column is full");
        }

        for (int i = 0; i < columnHeight; i++)
        {

            if (grid[cursorPosition, i] == 0)
            {
                grid[cursorPosition, i] = currentPlayer;
                if (i == columnHeight - 1)
                {
                    markColumnFull(cursorPosition);
                }

                Vector3 worldPos = new Vector3(transform.position.x + 0.175f, transform.position.y + (1 * i), transform.position.z + cursorPosition);
                return (worldPos, cursorPosition, i);
            }
        }
        UnityEngine.Debug.Log("landposerror");
        return (Vector3.zero, -1, -1);
    }

}
