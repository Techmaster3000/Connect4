using System.Diagnostics;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private int CurrentPlayer { get; set; } = 1;
    public Material Player1Material;
    public Material Player2Material;
    private Material currentMaterial;

    public void InitGame()
    {
        if (Player1Material == null || Player2Material == null)
        {
            UnityEngine.Debug.LogWarning("Player materials not assigned in GameStateManager.");
        }
        CurrentPlayer = 1;
        currentMaterial = Player1Material;
    }

    public void SwitchTurn(GameObject tokenPrefab, CursorHandler cursorHandler)
    {
        if (currentMaterial == Player1Material)
        {
            currentMaterial = Player2Material;
            CurrentPlayer = 2;
        }
        else
        {
            currentMaterial = Player1Material;
            CurrentPlayer = 1;
        }
        //change the material of the token prefab and cursor to the current player's material
        tokenPrefab.GetComponent<MeshRenderer>().material = currentMaterial;
        if (cursorHandler.cursor != null)
        {
            cursorHandler.cursor.GetComponent<MeshRenderer>().material = currentMaterial;
        }
    }

    public Material GetCurrentMaterial() => currentMaterial;
    public int GetCurrentPlayer() => CurrentPlayer;
}   
