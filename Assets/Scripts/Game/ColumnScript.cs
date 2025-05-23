using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameHandler : MonoBehaviour
{
    //variables entered through the main menu
    public int columnHeight;
    public uint gameMode;
    public int rowLength;
    public Material player1Material;
    public Material player2Material;

    //Prefabs
    public GameObject slotPrefab;
    public Material boardMaterial;
    public GameObject pillarPrefab;
    public GameObject pillarTopPrefab;
    public GameObject tokenPrefab;
    public GameObject cursorPrefab;

    private Material currentMaterial;
    private int cursorPosition = 0;
    
//    private GameObject token;
    private int currentPlayer;
    private int[,] grid;
    private GameObject[,] tokenGrid;
    private int[] fullColumns;
    private bool isBusy = false;
    public bool stopInput = false;

    private GameObject globalVolume;



    void Start()
    {
        initGame();

        createSlots();
        createPillars();
        
        initCursor();
        initTokens();

    }
    private void createSlots()
    {
       
        slotPrefab.transform.localScale = new Vector3(50, 50, 50);
        slotPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 0.50f, transform.position.z);

        for (int l = 0; l < rowLength; l++)
        {
            for (int i = 0; i < columnHeight; i++)
            {
                GameObject slot1 = Instantiate(slotPrefab, new Vector3(transform.position.x, transform.position.y + (1 * i), transform.position.z + (1 * l)), Quaternion.identity);
                GameObject slot2 = Instantiate(slotPrefab, new Vector3(transform.position.x + 0.25f, transform.position.y + (1 * i), transform.position.z + (1 * l)), Quaternion.identity);

                slot1.transform.localScale = new Vector3(50, 50, 50);
                slot2.transform.localScale = new Vector3(50, 50, 50);
                slot1.SetActive(true);
                slot2.SetActive(true);
            }
        }

        Destroy(slotPrefab, 0.1f);
    }
    private void initCursor()
    {
        cursorPrefab.transform.localScale = new Vector3(55f, 55f, 55f);
        cursorPrefab.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        cursorPrefab.GetComponent<MeshRenderer>().material = currentMaterial;
        Instantiate(cursorPrefab, new Vector3(transform.position.x + 0.25f, transform.position.y + 1 + (1 * columnHeight), transform.position.z + (rowLength / 2)), Quaternion.Euler(270, 0, 0));
    }
    private void initTokens()
    {

        MeshRenderer tokenRender = tokenPrefab.GetComponent<MeshRenderer>();
        tokenRender.material = currentMaterial;
        tokenPrefab.transform.localScale = new Vector3(100, 100, 50);

        tokenPrefab.SetActive(false);

    }
    private void createPillars()
    {
        MeshRenderer pillarRender = pillarPrefab.GetComponent<MeshRenderer>();
        pillarRender.material = boardMaterial;
        pillarPrefab.transform.localScale = new Vector3(50, 50, 50);
        pillarPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 0.50f, transform.position.z);
        pillarPrefab.transform.Rotate(0, 0, 90);
        for (int b = 0; b < columnHeight; b++)
        {
           GameObject pillar = Instantiate(pillarPrefab, new Vector3(transform.position.x, transform.position.y + (1 * b), transform.position.z - 1), Quaternion.Euler(90, 0, 0));
            pillar.SetActive(true);
        }
        createPillarsTop();
        for (int b = 0; b < columnHeight; b++)
        {
            GameObject pillar = Instantiate(pillarPrefab, new Vector3(transform.position.x, transform.position.y + (1 * b), transform.position.z + rowLength), Quaternion.Euler(90, 0, 0));
            pillar.SetActive(true);
        }
        Destroy(pillarPrefab, 0.1f);
        
    }
    private void createPillarsTop()
    {
        MeshRenderer pillarTopRender = pillarTopPrefab.GetComponent<MeshRenderer>();
        pillarTopRender.material = boardMaterial;
        pillarTopPrefab.transform.localScale = new Vector3(50, 50, 50);
        pillarTopPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z);
        Instantiate(pillarTopPrefab, new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z - 1), Quaternion.Euler(270, 0, 0));
       
        Instantiate(pillarTopPrefab, new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z + rowLength), Quaternion.Euler(270, 0, 0));

        Destroy(pillarTopPrefab, 0.1f);
    }
    private void initGame()
    {
        isBusy = true;
        tokenGrid = new GameObject[rowLength, columnHeight];

        currentPlayer = 1;
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

        globalVolume = GameObject.Find("GlobalVolume");
        fullColumns = new int[rowLength];
        for (int i = 0; i < fullColumns.Length; i++)
        {
            fullColumns[i] = 0;
        }
        grid = new int[rowLength, columnHeight];
        for (int i = 0; i < columnHeight; i++)
        {
            for (int j = 0; j < rowLength; j++)
            {
                grid[j, i] = 0;
            }
        }

        try
        {
            ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Player1Color"), out Color player1Color);
            player1Material.color = player1Color;
            ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Player2Color"), out Color player2Color);
            player2Material.color = player2Color;
            currentMaterial = player1Material;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error getting Player Color from PlayerPrefs: " + e.Message);
            currentMaterial = player1Material;
        }
        isBusy = false;

    }
    private void playerTurn()
    {

        if (currentMaterial == player1Material)
        {
            currentMaterial = player2Material;
            currentPlayer = 2;
        }
        else
        {
            currentMaterial = player1Material;
            currentPlayer = 1;
        }

        tokenPrefab.GetComponent<MeshRenderer>().material = currentMaterial;

        GameObject cursor = GameObject.Find("pointer(Clone)");
        if (cursor != null)
        {
            cursor.GetComponent<MeshRenderer>().material = currentMaterial;
        }

    }


    public void Update()
    {
        if (Time.timeScale == 0f || stopInput) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cursorPosition--;
            if (cursorPosition < 0)
            {
                cursorPosition = rowLength - 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cursorPosition++;
            if (cursorPosition >= rowLength)
            {
                cursorPosition = 0;
            }
        }

        GameObject cursor = GameObject.Find("pointer(Clone)");
        if (cursor != null)
        {
            cursor.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y + 1 + (1 * columnHeight), transform.position.z + (cursorPosition));

            if (cursor.transform.position.y < transform.position.y + 0.50f)
            {
                cursor.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y + 1 + (1 * columnHeight), transform.position.z + (cursorPosition));
            }
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isBusy)
            {
                return;
            }

            isBusy = true;
            Vector3 spawnPos = new Vector3(transform.position.x + 0.175f, transform.position.y + (1 * columnHeight), transform.position.z + (cursorPosition));
            Vector3 landPos;
            int posX;
            int posY;
            try
            {
                (landPos, posX, posY) = getlandPos();
            }
            catch (System.Exception e)
            {
                return;
            }

            GameObject spawnedToken = Instantiate(tokenPrefab, new Vector3(transform.position.x + 0.175f, transform.position.y + (1 * columnHeight), transform.position.z + (cursorPosition)), Quaternion.Euler(0, 90, 0));
            spawnedToken.SetActive(true);
            try
            {
                StartCoroutine(dropToken(spawnedToken, landPos, posX, posY));
            }
            catch (System.Exception e)
            {
                Destroy(spawnedToken);
                isBusy = false;
                return;
            }

        }
    }
    private void markColumnFull(int colNum)
    {
        fullColumns[colNum] = 1;

    }
    private bool isColumnFull()
    {
        if (fullColumns[cursorPosition] == 1)
        {

            return true;
        }
        return false;
    }
    private bool isBoardFull()
    {
        if (Array.TrueForAll(fullColumns, x => x == 1))
        {

            return true;
        }
        return false;
    }
    private (Vector3, int, int) getlandPos()
    {
        if (isColumnFull())
        {
            isBusy = false;
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
        Debug.Log("landposerror");
        return (Vector3.zero, -1, -1);
    }
    private IEnumerator dropToken(GameObject tokenToDrop, Vector3 landPos, int posX, int posY)
    {
        float duration = 0.3f;
        float elapsed = 0f;

        Vector3 startPos = tokenToDrop.transform.position;
        Vector3 endPos = new Vector3(startPos.x, landPos.y, startPos.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = (float)Math.Pow((elapsed / duration), 2);

            tokenToDrop.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        tokenToDrop.transform.position = landPos;
        tokenGrid[posX, posY] = tokenToDrop;
        checkWin(posX, posY);
        playerTurn();
        isBusy = false;
    }

    private void checkWin(int posX, int posY)
    {
        isBusy = true;
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(1, 0),   // horizontal
        new Vector2Int(0, 1),   // vertical
        new Vector2Int(1, 1),   // bottom-left to top-right
        new Vector2Int(1, -1),  // top left to bottom-right
        };

        List<GameObject> winningTokens = new List<GameObject>();

        foreach (var dir in directions)
        {
            List<Vector2Int> winningCoords = new List<Vector2Int>();
            winningCoords.Add(new Vector2Int(posX, posY));

            // Forward direction
            int x = posX + dir.x;
            int y = posY + dir.y;
            while (x >= 0 && x < rowLength && y >= 0 && y < columnHeight && grid[x, y] == currentPlayer)
            {
                winningCoords.Add(new Vector2Int(x, y));
                x += dir.x;
                y += dir.y;
            }

            // Backward direction
            x = posX - dir.x;
            y = posY - dir.y;
            while (x >= 0 && x < rowLength && y >= 0 && y < columnHeight && grid[x, y] == currentPlayer)
            {
                winningCoords.Add(new Vector2Int(x, y));
                x -= dir.x;
                y -= dir.y;
            }

            if (winningCoords.Count >= gameMode)
            {
                // Save token GameObjects
                winningTokens.Clear();
                foreach (var coord in winningCoords)
                {
                    winningTokens.Add(tokenGrid[coord.x, coord.y]);
                }
                stopInput = true;
                StartCoroutine(HighlightTokens(winningTokens));
                return;
            }
        }

        if (isBoardFull())
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

        globalVolume.GetComponent<UIHandler>().showWinScreen("Player " + currentPlayer);
    }
}