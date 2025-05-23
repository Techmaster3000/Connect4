using System.Collections;
using UnityEngine;

public class TokenAnimator : MonoBehaviour
{
    public IEnumerator DropToPosition(GameObject token, Vector3 endPos, float duration = 0.3f)
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
}
