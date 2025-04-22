using UnityEngine;
using TMPro;

public class HeightSelect : MonoBehaviour
{
    protected int Height;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        string heightText = text.text;

        try
        {
            //convert the text to an int
            Height = int.Parse(heightText);
            if (Height < 5 || Height > 7)
            {
                throw new System.Exception("GameMode is not between 3 and 5");
            }
            PlayerPrefs.SetInt("Height", Height);

        }
        catch (System.Exception e)
        {
            Debug.LogError("Error getting GameMode from PlayerPrefs: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnLeftArrowClick()
    {
        //decrease the game mode
        Height--;
        
        if (Height < 5)
        {
            Height = 5;
        }
        PlayerPrefs.SetInt("Height", Height);
        //set the text to the game mode
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = Height.ToString();
    }
    public void OnRightArrowClick()
    {
        //increase the game mode
        Height++;
        
        if (Height > 7)
        {
            Height = 5;
        }
        PlayerPrefs.SetInt("Height", Height);
        //set the text to the game mode
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = Height.ToString();
    }
}
