using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Collections.Specialized;

public class GameHandler : MonoBehaviour
{
    //Other Scripts
    private ObjectInitializer ObjInitializer;
    private BoardManager boardMan;
    private WinChecker winChecker;
    private TokenAnimator tokenAnimator;
    private InputHandler inputHandler;
    private GameStateManager gameStateMan;

    //variables entered through the main menu
    public int columnHeight;
    public uint gameMode;
    public int rowLength;
    public Material player1Material;
    public Material player2Material;

    public GameObject tokenPrefab;
    private MeshRenderer tokenRender;

    private int cursorPosition = 0;
    private GameObject cursor;

    public Material boardMaterial;
    private GameObject[,] tokenGrid;
    public bool isBusy = false;
    public bool stopInput = false;

    [SerializeField] private GameObject globalVolume;



    void Start()
    {
        boardMan = GetComponent<BoardManager>();
        gameStateMan = GetComponent<GameStateManager>();
        initGame();

        boardMan.columnHeight = columnHeight;
        boardMan.rowLength = rowLength;
        ObjInitializer = GetComponent<ObjectInitializer>();

        ObjInitializer.boardMaterial = boardMaterial;

        ObjInitializer.createSlots(rowLength, columnHeight);
        ObjInitializer.createPillars(rowLength, columnHeight);

        cursor = ObjInitializer.initCursor(gameStateMan.GetCurrentMaterial(), rowLength, columnHeight);
        tokenPrefab = ObjInitializer.initTokens(gameStateMan.GetCurrentMaterial());
        tokenRender = tokenPrefab.GetComponent<MeshRenderer>();

        winChecker = GetComponent<WinChecker>();
        tokenAnimator = GetComponent<TokenAnimator>();
        
        inputHandler = GetComponent<InputHandler>();
        inputHandler.Initialize(this);

        UpdateCursorPosition();
    }
    
    
    private void initGame()
    {
        gameStateMan.InitGame();
        isBusy = true;
        
        gameMode = (uint)PlayerPrefs.GetInt("GameMode");
        switch (gameMode)
        {
            case 3:
                rowLength = 6;
                break;
            case 4:
                rowLength = 7;
                break;
            case 5:
                rowLength = 8;
                break;
            default:
                rowLength = 6;
                break;
        }
        columnHeight = PlayerPrefs.GetInt("Height");
        tokenGrid = new GameObject[rowLength, columnHeight];


        boardMan.fullColumns = new int[rowLength];
        for (int i = 0; i < boardMan.fullColumns.Length; i++)
        {
            boardMan.fullColumns[i] = 0;
        }
        boardMan.grid = new int[rowLength, columnHeight];
        for (int i = 0; i < columnHeight; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {
                boardMan.grid[j, i] = 0;
            }
        }

        try
        {
            ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Player1Color"), out Color player1Color);
            player1Material.color = player1Color;
            ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Player2Color"), out Color player2Color);
            player2Material.color = player2Color;

            gameStateMan.Player1Material = player1Material;
            gameStateMan.Player2Material = player2Material;

            gameStateMan.InitGame();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error getting Player Color from PlayerPrefs: " + e.Message);
        }
        isBusy = false;

    }


    private void Update()
    {
        if (Time.timeScale == 0f || stopInput) return;

        inputHandler.HandleInput();
    }

    public void DecrementCursor()
    {
        cursorPosition = (cursorPosition - 1 + rowLength) % rowLength;
    }

    public void IncrementCursor()
    {
        cursorPosition = (cursorPosition + 1) % rowLength;
    }

    public void UpdateCursorPosition()
    {
        if (cursor != null)
        {
            float yPos = transform.position.y + 1 + (1 * columnHeight);
            float zPos = transform.position.z + cursorPosition;
            cursor.transform.position = new Vector3(transform.position.x + 0.25f, yPos, zPos);
        }
    }
    public void tryDropToken()
    {
        if (isBusy) return;

        isBusy = true;
        Vector3 spawnPos = new Vector3(transform.position.x + 0.175f, transform.position.y + (1 * columnHeight), transform.position.z + cursorPosition);
        Vector3 landPos;
        int posX, posY;

        try
        {
            (landPos, posX, posY) = boardMan.getlandPos(gameStateMan.GetCurrentPlayer(), cursorPosition);
        }
        catch (Exception)
        {
            isBusy = false;
            return;
        }

        GameObject spawnedToken = Instantiate(tokenPrefab, spawnPos, Quaternion.Euler(0, 90, 0));
        spawnedToken.SetActive(true);

        try
        {
            StartCoroutine(dropToken(spawnedToken, landPos, posX, posY));
        }
        catch (Exception)
        {
            Destroy(spawnedToken);
            isBusy = false;
        }
    }

    private IEnumerator dropToken(GameObject tokenToDrop, Vector3 landPos, int posX, int posY)
    {
        yield return StartCoroutine(tokenAnimator.DropToPosition(tokenToDrop, landPos));

        tokenToDrop.transform.position = landPos;
        tokenGrid[posX, posY] = tokenToDrop;

        checkWin(posX, posY);


        gameStateMan.SwitchTurn();

        tokenPrefab.GetComponent<MeshRenderer>().material = gameStateMan.GetCurrentMaterial();
        if (cursor != null)
            cursor.GetComponent<MeshRenderer>().material = gameStateMan.GetCurrentMaterial();

        isBusy = false;
    }


    private void checkWin(int posX, int posY)
    {
        isBusy = true;

        if (WinChecker.TryGetWinningTokens(boardMan.grid, tokenGrid, gameStateMan.GetCurrentPlayer(), (int)gameMode, posX, posY, out var winningTokens))
        {
            stopInput = true;
            StartCoroutine(HighlightTokens(winningTokens));
            return;
        }

        if (boardMan.isBoardFull())
        {
            globalVolume.GetComponent<UIHandler>().showWinScreen("draw");
        }
    }

    private IEnumerator HighlightTokens(List<GameObject> winningTokens)
    {
        yield return new WaitForSeconds(0.2f);
        foreach (var token in winningTokens)
        {
            Material highlightMaterial = new Material(token.GetComponent<Renderer>().material);
            highlightMaterial.EnableKeyword("_EMISSION");
            highlightMaterial.SetColor("_EmissionColor", Color.white * 2f);
            token.GetComponent<Renderer>().material = highlightMaterial;

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        globalVolume.GetComponent<UIHandler>().showWinScreen("Player " + gameStateMan.GetCurrentPlayer());
    }
}