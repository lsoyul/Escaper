using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;

public class LightBlink : MonoBehaviour
{
    public Light2D targetLight;
    public TweenValue tweenValue;

    bool isFinish = false;

    public float StartValue = -1f;
    public float EndValue = -1f;

    public void StartBlink(float duration, Color targetColor)
    {
        this.gameObject.SetActive(true);
        isFinish = false;
        tweenValue.Begin();

        if (StartValue != -1 && EndValue != -1)
        {
            tweenValue.startValue = StartValue;
            tweenValue.endValue = EndValue;
        }

        targetLight.intensity = tweenValue.startValue;
        targetLight.color = targetColor;
        StartCoroutine(Timer(duration));
    }

    public void StartBlink()
    {
        this.gameObject.SetActive(true);
        isFinish = false;
        tweenValue.myTweenType = TweenBase.playStyles.PingPong;
        tweenValue.Begin(); 
        
        if (StartValue != -1 && EndValue != -1)
        {
            tweenValue.startValue = StartValue;
            tweenValue.endValue = EndValue;
        }

        targetLight.intensity = tweenValue.startValue;
    }

    public void Hide()
    {
        tweenValue.Reset();
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
            tweenValue.Reset();
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
