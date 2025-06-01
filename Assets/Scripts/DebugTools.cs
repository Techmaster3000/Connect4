using UnityEngine;
using UnityEngine.UI;


public class DebugTools : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Fps Limit: " + Application.targetFrameRate);
        }

    }
}
