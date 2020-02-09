using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;

public class LightBlink : MonoBehaviour
{
    public Light2D targetLight;
    public TweenValue tweenValue;

    bool isFinish = false;

    public void StartBlink(float duration)
    {
        this.gameObject.SetActive(true);
        isFinish = false;
        targetLight.intensity = tweenValue.startValue;
        tweenValue.Begin();
        StartCoroutine(Timer(duration));
    }

    public void StartBlink()
    {
        this.gameObject.SetActive(true);
        isFinish = false;
        tweenValue.myTweenType = TweenBase.playStyles.PingPong;
        tweenValue.Begin();
        targetLight.intensity = tweenValue.startValue;
    }

    public void Hide()
    {
        isFinish = true;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        targetLight.intensity = tweenValue.value;
        if (isFinish)
        {
            tweenValue.Completed();
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator Timer(float duration)
    {
        float curTime = 0f;

        while (curTime < duration)
        {
            curTime += Time.deltaTime;
            yield return null;
        }

        isFinish = true;
    }
}
