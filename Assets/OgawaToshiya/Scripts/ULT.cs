using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULT : MonoBehaviour
{
    private Vector3 initialScale;

    //
    public float scaleDuration = 2f;
    public float targetScale = 2f;

    void Start()
    {
        initialScale = transform.localScale;

        //
        StartCoroutine(ScaleOverTime());
    }

    // 
    private IEnumerator ScaleOverTime()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScaleVector = originalScale * targetScale;
        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            float t = elapsedTime / scaleDuration;
            transform.localScale = Vector3.Lerp(originalScale, targetScaleVector, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScaleVector;
    }

    public void ResetToInitialSize()
    {
        transform.localScale = initialScale;
    }
}
