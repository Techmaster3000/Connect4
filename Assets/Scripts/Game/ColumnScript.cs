using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using System;   

public class Slot : MonoBehaviour
{
    public int columnHeight;
    public int rowLength;
    public Mesh slotMesh;
    public Material boardMaterial;
    public Mesh pillarMesh;
    public Mesh pillarTopMesh;
    public Mesh tokenMesh;
    public Material player1Material;
    public Material player2Material;
    private Material currentMaterial;
    private int cursorPosition = 0;
    public GameObject cursorPrefab;
    private GameObject token;
    private int currentPlayer;
    private int[,] grid;
    private int[] fullColumns;
    private bool isBusy = false;
    public uint gameMode;
    private GameObject globalVolume;


    void Start()
    {
        
        initGame();
        GameObject slotPrefab = new GameObject("Slot");
        MeshFilter filter = slotPrefab.AddComponent<MeshFilter>();
        filter.mesh = slotMesh;
        MeshRenderer render = slotPrefab.AddComponent<MeshRenderer>();
        render.material = boardMaterial;
        slotPrefab.transform.localScale = new Vector3(50, 50, 50);
        slotPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 0.50f, transform.position.z);

        // Create the pillar
        GameObject pillarPrefab = new GameObject("Pillar");
        MeshFilter pillarFilter = pillarPrefab.AddComponent<MeshFilter>();
        pillarFilter.mesh = pillarMesh;
        MeshRenderer pillarRender = pillarPrefab.AddComponent<MeshRenderer>();
        pillarRender.material = boardMaterial;
        pillarPrefab.transform.localScale = new Vector3(50, 50, 50);
        pillarPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 0.50f, transform.position.z);
        pillarPrefab.transform.Rotate(0, 0, 90);

        for (int b = 0; b < columnHeight; b++)
        {
            //rotate the pillar piece

            Instantiate(pillarPrefab, new Vector3(transform.position.x, transform.position.y + (1 * b), transform.position.z - 1), Quaternion.Euler(90, 0, 0));
        }
        // Create the top of the pillar
        GameObject pillarTopPrefab = new GameObject("PillarTop");
        MeshFilter pillarTopFilter = pillarTopPrefab.AddComponent<MeshFilter>();
        pillarTopFilter.mesh = pillarTopMesh;
        MeshRenderer pillarTopRender = pillarTopPrefab.AddComponent<MeshRenderer>();
        pillarTopRender.material = boardMaterial;
        pillarTopPrefab.transform.localScale = new Vector3(50, 50, 50);
        pillarTopPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z);
        Instantiate(pillarTopPrefab, new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z - 1), Quaternion.Euler(270, 0, 0));
        for (int l = 0; l < rowLength; l++)
        {
            for (int i = 0; i < columnHeight; i++)
            {
                Instantiate(slotPrefab, new Vector3(transform.position.x, transform.position.y + (1 * i), transform.position.z + (1 * l)), Quaternion.identity);
                Instantiate(slotPrefab, new Vector3(transform.position.x + 0.25f, transform.position.y + (1 * i), transform.position.z + (1 * l)), Quaternion.identity);
            }
        }
        for (int b = 0; b < columnHeight; b++)
        {
            //rotate the pillar piece

            Instantiate(pillarPrefab, new Vector3(transform.position.x, transform.position.y + (1 * b), transform.position.z + rowLength), Quaternion.Euler(90, 0, 0));
        }
        Instantiate(pillarTopPrefab, new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z + rowLength), Quaternion.Euler(270, 0, 0));
        Destroy(slotPrefab, 0.1f);
        Destroy(pillarPrefab, 0.1f);
        Destroy(pillarTopPrefab, 0.1f);
        cursorPrefab.transform.localScale = new Vector3(55f, 55f, 55f);
        cursorPrefab.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        cursorPrefab.GetComponent<MeshRenderer>().material = currentMaterial;
        Instantiate(cursorPrefab, new Vector3(transform.position.x + 0.25f, transform.position.y + 1 + (1 * columnHeight), transform.position.z + (rowLength / 2)), Quaternion.Euler(270, 0, 0));
        token = new GameObject("Token");
        MeshFilter tokenFilter = token.AddComponent<MeshFilter>();
        tokenFilter.mesh = tokenMesh;
        MeshRenderer tokenRender = token.AddComponent<MeshRenderer>();
        tokenRender.material = currentMaterial;
        token.transform.localScale = new Vector3(100, 100, 50);
        //add a rigidbody to the token
        Rigidbody tokenRigidbody = token.AddComponent<Rigidbody>();
        tokenRigidbody.mass = 1f;
        tokenRigidbody.isKinematic = false;
        tokenRigidbody.useGravity = false;
        tokenRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //make the token inactive but make the instance of the token active
        token.SetActive(false);

    }
    private void initGame()
    {
        isBusy = true;

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

        //set the materials
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
        // Switch the material for the next player's turn
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
        // Update the token's material
        token.GetComponent<MeshRenderer>().material = currentMaterial;
        // Update the cursor's material
        GameObject cursor = GameObject.Find("pointer(Clone)");
        if (cursor != null)
        {
            cursor.GetComponent<MeshRenderer>().material = currentMaterial;
        }

    }


    public void Update()
    {
        if (Time.timeScale == 0f) return;
        // Update the cursor position based on user input
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
        //move the cursor instance
        GameObject cursor = GameObject.Find("pointer(Clone)");
        if (cursor != null)
        {
            cursor.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y + 1 + (1 * columnHeight), transform.position.z + (cursorPosition));
            //do not let the token get below the slot
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
            //drop a token in the selected column
            GameObject spawnedToken = Instantiate(token, new Vector3(transform.position.x + 0.175f, transform.position.y + (1 * columnHeight), transform.position.z + (cursorPosition)), Quaternion.Euler(0, 90, 0));
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
        //if all columns are full, run the showWinscreen from the global volume
    }
    private bool isColumnFull()
    {
        if (fullColumns[cursorPosition] == 1)
        {
            //if the column is full, return true
            return true;
        }
        return false;
    }
    private bool isBoardFull()
    {
        if (Array.TrueForAll(fullColumns, x => x == 1))
        {
            //if the board is full, return true
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
        return (Vector3.zero, -1, -1); // fallback
    }
    private IEnumerator dropToken(GameObject tokenToDrop, Vector3 landPos, int posX, int posY)
    {
        float duration = 0.3f; // Time to reach the destination
        float elapsed = 0f;

        Vector3 startPos = tokenToDrop.transform.position;
        Vector3 endPos = new Vector3(startPos.x, landPos.y, startPos.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = (float)Math.Pow((elapsed / duration), 2);
            // accelerate the drop
          

            tokenToDrop.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        tokenToDrop.transform.position = landPos;

        checkWin(posX, posY);
        playerTurn();
        isBusy = false;
    }
    private void checkWin(int posX, int posY) {
        Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),   
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),  
    };

        foreach (var dir in directions)
        {
            int count = 1;
            count += countDirection(dir.x, dir.y, posX, posY);
            count += countDirection(-dir.x, -dir.y, posX, posY);

            if (count >= gameMode)
            {
                //run the showWinscreen from the global volume
                globalVolume.GetComponent<UIHandler>().showWinScreen("Player " + currentPlayer);
                return;
            }
            else if (isBoardFull())
            {
                globalVolume.GetComponent<UIHandler>().showWinScreen("draw");
                return;
            }
        }
    }
    private int countDirection(int dx, int dy, int posX, int posY) {
        int count = 0;
        int x = posX + dx;
        int y = posY + dy;

        while (x >= 0 && x < rowLength && y >= 0 && y < columnHeight && grid[x, y] == currentPlayer)
        {
            count++;
            x += dx;
            y += dy;
        }

        return count;
    }
}
