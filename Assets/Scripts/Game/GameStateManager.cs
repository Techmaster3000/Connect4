using System.Diagnostics;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private int CurrentPlayer = 1;
    public Material Player1Material;
    public Material Player2Material;
    private Material currentMaterial;
    public bool isSinglePlayer;
    private AIPlayer aiPlayer;

    public void InitGame()
    {
        if (Player1Material == null || Player2Material == null)
        {
            UnityEngine.Debug.LogWarning("Player materials not assigned in GameStateManager.");
        }
        CurrentPlayer = 1;
        currentMaterial = Player1Material;
        isSinglePlayer = PlayerPrefs.GetInt("Players") == 1;

        if (PlayerPrefs.GetInt("IsSinglePlayer", 0) == 1)
        {
            isSinglePlayer = true;
            aiPlayer = GetComponent<AIPlayer>();
            if (aiPlayer == null)
            {
                UnityEngine.Debug.LogError("AIPlayer component not found on GameStateManager.");
            }
            aiPlayer.initAI(GetComponent<BoardManager>(), GetComponent<CursorHandler>(), GetComponent<InputHandler>());
        }

        else
        {
            isSinglePlayer = false;
        }
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
        InputHandler inputHandler = GetComponent<InputHandler>();
        if (isSinglePlayer && CurrentPlayer == 2)
        {
            inputHandler.aiTurn = true;
            //set IsBusy to true
            GetComponent<GameHandler>().isBusy = false;
            aiPlayer.MakeMove();
        }
        else
        {
            inputHandler.aiTurn = false;
        }
    }

    public Material GetCurrentMaterial() => currentMaterial;
    public int GetCurrentPlayer() => CurrentPlayer;
}   
