using UnityEngine;

public class ColorSelect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setPlayer1Color("red");
        setPlayer2Color("orange");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setPlayer1Color(string color)
    {
        string hexCode = fetchHex(color);
        PlayerPrefs.SetString("Player1Color", hexCode);
        Debug.Log("Player 1 Color: " + hexCode);
    }
    public void setPlayer2Color(string color)
    {
        //set the player 2 color
        string hexCode = fetchHex(color);
        PlayerPrefs.SetString("Player2Color", hexCode);
        Debug.Log("Player 2 Color: " + hexCode);
    }
    private string fetchHex(string color)
    {
        //get the color of the game object
        switch(color)
        {
            case "red":
                return "#FF0000";
            case "green":
                return "#00FF00";
            case "yellow":
                return "#FFFF00";
            case "white":
                return "#FFFFFF";
            case "black":
                return "#000000";
            case "sky":
                return "#00CCFF";
            case "pink":
                return "#FF5FD5";
            case "purple":
                return "#9500FF";
            case "orange":
                return "#FF8B00";
            case "teal":
                return "#008C9D";
            default:
                Debug.LogError("Color not found: " + color);
                return "#FFFFFF"; // Default to white if color not found
        }
    }
}
