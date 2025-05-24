using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    public GameObject cursor;
    public int cursorPosition = 0;
    public int rowLength = 7; // Default value, can be set externally
    public int columnHeight = 6; // Default value, can be set externally


    public void Initialize(int rowLength, int columnHeight)
    {
        this.rowLength = rowLength;
        this.columnHeight = columnHeight;
        if (cursor != null)
        {
            UpdateCursorPosition();
        }
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

}
