using UnityEngine;
using UnityEngine.UI;

public class PlayerModeSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerSelect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get the toggle component of the game object
        Toggle playerToggle = PlayerSelect.GetComponent<Toggle>();
        if (playerToggle != null)
        {
            // Check PlayerPrefs for the saved player mode
            int isSinglePlayer = PlayerPrefs.GetInt("IsSinglePlayer", 0); // Default to single player if not set
            playerToggle.isOn = (isSinglePlayer == 1);
        }
        else
        {
            Debug.LogError("PlayerSelect GameObject does not have a Toggle component.");
        }




    }


    public void onToggleClicked() 
    {
        //add a boolean to Playerprefs to save the player mode
        bool isSinglePlayer = PlayerSelect.GetComponent<Toggle>().isOn;
        PlayerPrefs.SetInt("IsSinglePlayer", isSinglePlayer ? 1 : 0);




    }

}
