using UnityEngine;
using System.Collections;
using System;

public class GameHandler : MonoBehaviour
{
    // Other Scripts
    private ObjectInitializer ObjInitializer;
    private BoardManager boardMan;
    private InputHandler inputHandler;
    private GameStateManager gameStateMan;
    private CursorHandler cursorHandler;

    // Variables entered through the main menu
    public int columnHeight;
    public uint gameMode;
    public int rowLength;
    public Material player1Material;
    public Material player2Material;

    public GameObject tokenPrefab;
    public Material boardMaterial;
    public bool isBusy = false;
    public bool stopInput = false;

    [SerializeField] private GameObject globalVolume;

    void Start()
    {
        Application.targetFrameRate = PlayerPrefs.GetInt("FpsCap", 60);
        // Cache GetComponent calls
        boardMan = GetComponent<BoardManager>();
        gameStateMan = GetComponent<GameStateManager>();
        ObjInitializer = GetComponent<ObjectInitializer>();
        cursorHandler = GetComponent<CursorHandler>();
        inputHandler = GetComponent<InputHandler>();
        inputHandler.Initialize(cursorHandler);
        initGame();

        boardMan.columnHeight = columnHeight;
        boardMan.rowLength = rowLength;
        ObjInitializer.boardMaterial = boardMaterial;

        ObjInitializer.createSlots(rowLength, columnHeight);
        ObjInitializer.createPillars(rowLength, columnHeight);

        ObjInitializer.initCursor(gameStateMan.GetCurrentMaterial(), rowLength, columnHeight, cursorHandler);
        tokenPrefab = ObjInitializer.initTokens(gameStateMan.GetCurrentMaterial());

        cursorHandler.Initialize(rowLength, columnHeight);
        cursorHandler.UpdateCursorPosition();
    }

    private void initGame()
    {
        isBusy = true;
        gameStateMan.InitGame();

        gameMode = (uint)PlayerPrefs.GetInt("GameMode");
        rowLength = gameMode switch
        {
            3 => 6,
            4 => 7,
            5 => 8,
            _ => 6
        };
        columnHeight = PlayerPrefs.GetInt("Height");

        boardMan.tokenGrid = new GameObject[rowLength, columnHeight];
        boardMan.fullColumns = new int[rowLength];
        boardMan.grid = new int[rowLength, columnHeight];

        try
        {
            if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Player1Color"), out Color player1Color))
                player1Material.color = player1Color;
            if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Player2Color"), out Color player2Color))
                player2Material.color = player2Color;

            gameStateMan.Player1Material = player1Material;
            gameStateMan.Player2Material = player2Material;

            gameStateMan.InitGame();
        }
        catch (Exception e)
        {
            Debug.LogError("Error getting Player Color from PlayerPrefs: " + e.Message);
        }
        isBusy = false;
    }

    private void Update()
    {
        
        inputHandler.HandleInput(stopInput);
    }

    public void tryDropToken()
    {
        if (isBusy) { Debug.Log("Game is busy, cannot drop token."); return; }
        isBusy = true;

        Vector3 spawnPos = new Vector3(
            transform.position.x + 0.175f,
            transform.position.y + (1 * columnHeight),
            transform.position.z + cursorHandler.cursorPosition
        );

        try
        {
            var (landPos, posX, posY) = boardMan.getlandPos(gameStateMan.GetCurrentPlayer(), cursorHandler.cursorPosition);
            GameObject spawnedToken = Instantiate(tokenPrefab, spawnPos, Quaternion.Euler(0, 90, 0));
            spawnedToken.SetActive(true);
            StartCoroutine(dropToken(spawnedToken, landPos, posX, posY));
        }
        catch (Exception)
        {
            isBusy = false;
        }
    }

    private IEnumerator dropToken(GameObject tokenToDrop, Vector3 landPos, int posX, int posY)
    {
        yield return TokenAnimator.DropToPosition(tokenToDrop, landPos);
        tokenToDrop.transform.position = landPos;
        boardMan.tokenGrid[posX, posY] = tokenToDrop;

        bool gameOver = checkWin(posX, posY);

        if (!gameOver)
        {
            gameStateMan.SwitchTurn(tokenPrefab, cursorHandler);
        }
        isBusy = false;
    }

    private bool checkWin(int posX, int posY)
    {
        isBusy = true;

        int winningPlayer = gameStateMan.GetCurrentPlayer();

        if (WinChecker.TryGetWinningTokens(boardMan.grid, boardMan.tokenGrid, winningPlayer, (int)gameMode, posX, posY, out var winningTokens))
        {
            stopInput = true;
            StartCoroutine(TokenAnimator.HighlightTokens(winningTokens, globalVolume, gameStateMan, winningPlayer));
            return true; // Game over
        }

        if (boardMan.isBoardFull())
        {
            globalVolume.GetComponent<UIHandler>().showWinScreen("draw");
            return true; // Game over
        }

        return false; // Game continues
    }
}