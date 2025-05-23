using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    private static readonly Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),   // horizontal
        new Vector2Int(0, 1),   // vertical
        new Vector2Int(1, 1),   // diagonal TL-BR
        new Vector2Int(1, -1),  // diagonal BL-TR
    };

    public static bool TryGetWinningTokens(int[,] grid, GameObject[,] tokenGrid, int currentPlayer, int winLength, int posX, int posY, out List<GameObject> winningTokens)
    {
        winningTokens = new List<GameObject>();

        foreach (var dir in directions)
        {
            List<Vector2Int> coords = new List<Vector2Int> { new Vector2Int(posX, posY) };

            // Forward
            int x = posX + dir.x;
            int y = posY + dir.y;
            while (IsInBounds(grid, x, y) && grid[x, y] == currentPlayer)
            {
                coords.Add(new Vector2Int(x, y));
                x += dir.x;
                y += dir.y;
            }

            // Backward
            x = posX - dir.x;
            y = posY - dir.y;
            while (IsInBounds(grid, x, y) && grid[x, y] == currentPlayer)
            {
                coords.Add(new Vector2Int(x, y));
                x -= dir.x;
                y -= dir.y;
            }

            if (coords.Count >= winLength)
            {
                winningTokens.Clear();
                foreach (var coord in coords)
                {
                    winningTokens.Add(tokenGrid[coord.x, coord.y]);
                }
                return true;
            }
        }

        return false;
    }

    private static bool IsInBounds(int[,] grid, int x, int y)
    {
        return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
    }
}
