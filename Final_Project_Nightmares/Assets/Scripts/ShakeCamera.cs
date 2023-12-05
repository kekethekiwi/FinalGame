using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    float shakeAmt = 0;
    float shakeDuration = 0;
    float startAmt = 0;
    float startDuration = 0;
    bool isRunning = false;
    //private readonly float

    public void ShakeTheCamera(float amt, float duration)
    {
        shakeAmt += amt;
        startAmt = shakeAmt;
        shakeDuration += duration;
        startDuration = shakeDuration;
        if (!isRunning) StartCoroutine(Shake());
    }
    

    IEnumerator Shake()
    {
        isRunning = true;
        while (shakeDuration > 0.005f && shakeAmt > 0.001f)
        {
            Vector3 shakeVector = Random.insideUnitSphere * (shakeAmt);
            float shakePercentage = shakeDuration / startDuration;
            shakeAmt = startAmt * shakePercentage;
            shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);
            transform.localPosition = shakeVector;
            yield return null;
        }
        shakeAmt = 0f;
        shakeDuration = 0;
        transform.localPosition = Vector3.zero;
        isRunning = false;   
    }

}
