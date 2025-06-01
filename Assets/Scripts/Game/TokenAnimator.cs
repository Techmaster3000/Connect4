using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenAnimator : MonoBehaviour
{
    public static IEnumerator DropToPosition(GameObject token, Vector3 endPos, float duration = 0.3f)
    {
        float elapsed = 0f;
        Vector3 startPos = token.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = t * t;

            token.transform.position = Vector3.Lerp(startPos, endPos, easedT);
            yield return null;
        }

        token.transform.position = endPos;
    }

    public static IEnumerator HighlightTokens(List<GameObject> winningTokens, GameObject globalVolume, GameStateManager gameStateMan, int winningPlayer)
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

        globalVolume.GetComponent<UIHandler>().showWinScreen("Player " + winningPlayer);
    }
}
