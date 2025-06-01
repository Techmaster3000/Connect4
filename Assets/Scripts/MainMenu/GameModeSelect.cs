using UnityEngine;
using TMPro;

public class GameModeSelect : MonoBehaviour
{
    protected int GameMode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get the text component of the game object
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        string gameModeText = text.text;
        try
        {
            //convert the text to an int
            GameMode = int.Parse(gameModeText);
            if (GameMode < 3 || GameMode > 5)
            {
                GameMode = 4;
                text.text = GameMode.ToString();
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError("Error getting GameMode:" + e.Message);
            GameMode = 4; // Default to 0 if there's an error
        }
        PlayerPrefs.SetInt("GameMode", GameMode);
        //set the text to the game mode

    }
    public void OnLeftArrowClick()
    {
        //decrease the game mode
        GameMode--;
        if (GameMode < 3)
        {
            GameMode = 5;
        }
        PlayerPrefs.SetInt("GameMode", GameMode);
        //set the text to the game mode
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = GameMode.ToString();
    }
    public void OnRightArrowClick()
    {
        //increase the game mode
        GameMode++;
        if (GameMode > 5)
        {
            GameMode = 3;
        }
        PlayerPrefs.SetInt("GameMode", GameMode);
        //set the text to the game mode
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = GameMode.ToString();
    }
}
