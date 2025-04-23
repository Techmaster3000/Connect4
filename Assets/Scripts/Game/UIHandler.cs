using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;


public class UIHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject winScreen;
    private DepthOfField dof;
    public Volume globalVolume;
    private Coroutine blurCoroutine;

    void Start()
    {
        // Find the UIScreen GameObject in the scene

        winScreen = GameObject.Find("UIScreen");
        if (winScreen != null)
        {
            //set the opacity to 0
            winScreen.GetComponent<CanvasGroup>().alpha = 0;
            winScreen.SetActive(false); // Deactivate the UIScreen at the start
        }
        else
        {
            Debug.LogError("UIScreen not found in the scene.");
        }

        globalVolume.profile.TryGet<DepthOfField>(out dof);
        if (dof != null)
        {
            dof.focusDistance.value = 10f; // Set the focus distance to 0.5 meters
        }


    }

    // Update is called once per frame
    void Update()
    {
        // Check if the user is pressing the "Escape" key


    }
    public void StartBlur()
    {
        if (blurCoroutine != null)
        {
            StopCoroutine(blurCoroutine);
        }
        blurCoroutine = StartCoroutine(BlurTo(0.1f)); // blur in
    }

    public void StopBlur()
    {
        if (blurCoroutine != null)
        {
            StopCoroutine(blurCoroutine);
        }
        blurCoroutine = StartCoroutine(BlurTo(10f)); // blur out
    }

    private IEnumerator BlurTo(float targetValue)
    {
        if (dof != null)
        {
            float start = dof.focusDistance.value;
            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsed / duration);
                dof.focusDistance.value = Mathf.Lerp(start, targetValue, t);
                yield return null;
            }

            dof.focusDistance.value = targetValue; // Snap to final value
        }

        blurCoroutine = null;
    }
    public IEnumerator fadeUI()
    {
        if (winScreen != null)
        {
            float start = winScreen.GetComponent<CanvasGroup>().alpha;
            float elapsed = 0f;
            float duration = 0.5f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsed / duration);
                winScreen.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, 1, t);
                yield return null;
            }
        }
    }
    public void showWinScreen(string player)
    {
        if (winScreen != null)
        {
            StartBlur();
            StartCoroutine(fadeUI());
            winScreen.SetActive(true);
            //change the text
            if (player == "draw")
            {
                winScreen.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Draw!";
            }
            else
            {
                winScreen.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = player + " Wins!";
            }
        }
        else
        {
            Debug.LogError("UIScreen not found in the scene.");
        }
        Time.timeScale = 0; // Pause the game
    }
}
