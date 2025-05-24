using UnityEngine;

public class ObjectInitializer : MonoBehaviour
{
    public GameObject cursorPrefab;
    public GameObject slotPrefab;
    public GameObject tokenPrefab;
    public GameObject pillarPrefab;
    public GameObject pillarTopPrefab;
    public Material boardMaterial;


    private static void initObject(GameObject Prefab, Material materialToSet, Vector3 localScale)
    {
        MeshRenderer Renderer = Prefab.GetComponent<MeshRenderer>();
        Renderer.material = materialToSet;
        Prefab.transform.localScale = localScale;

    }

    public void initCursor(Material currentMaterial, int rowLength, int columnHeight, CursorHandler cursorHandler)
    {
        initObject(cursorPrefab, currentMaterial, new Vector3(55f, 55f, 55f));
        cursorHandler.cursor = Instantiate(cursorPrefab, new Vector3(transform.position.x + 0.25f, transform.position.y + 1 + (1 * columnHeight), transform.position.z + (rowLength / 2)), Quaternion.Euler(270, 0, 0));

    }

    public void createSlots(int rowLength, int columnHeight)
    {
        initObject(slotPrefab, boardMaterial, new Vector3(50, 50, 50));
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
    }
    public GameObject initTokens(Material currentMaterial)
    {
        initObject(tokenPrefab, currentMaterial, new Vector3(100, 100, 50));

        tokenPrefab.SetActive(false);
        return tokenPrefab;

    }

    public void createPillars(int rowLength, int columnHeight)
    {
        initObject(pillarPrefab, boardMaterial, new Vector3(50, 50, 50));
        pillarPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + 0.50f, transform.position.z);
        pillarPrefab.transform.Rotate(0, 0, 90);
        for (int b = 0; b < columnHeight; b++)
        {
            GameObject pillar = Instantiate(pillarPrefab, new Vector3(transform.position.x, transform.position.y + (1 * b), transform.position.z - 1), Quaternion.Euler(90, 0, 0));
            pillar.SetActive(true);
        }
        createPillarsTop(rowLength, columnHeight, boardMaterial);
        for (int b = 0; b < columnHeight; b++)
        {
            GameObject pillar = Instantiate(pillarPrefab, new Vector3(transform.position.x, transform.position.y + (1 * b), transform.position.z + rowLength), Quaternion.Euler(90, 0, 0));
            pillar.SetActive(true);
        }

    }
    private void createPillarsTop(int rowLength, int columnHeight, Material boardMaterial)
    {
        initObject(pillarTopPrefab, boardMaterial, new Vector3(50, 50, 50));
        pillarTopPrefab.transform.position = new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z);
        Instantiate(pillarTopPrefab, new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z - 1), Quaternion.Euler(270, 0, 0));

        Instantiate(pillarTopPrefab, new Vector3(transform.position.x, transform.position.y + (1 * (columnHeight - 1)), transform.position.z + rowLength), Quaternion.Euler(270, 0, 0));

    }
}
