using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;

public class TweenLightIntensity : MonoBehaviour
{
    public Light2D light2d;
    public TweenValue tweenValue;
    public TweenValue tweenAngle;

    private bool angleTweenStart = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        tweenValue.TweenCompleted += () => {
            tweenAngle.Begin();
            angleTweenStart = true;
        };
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        light2d.intensity = tweenValue.value;
        if (angleTweenStart) 
        {
            light2d.pointLightInnerAngle = 1f;
            light2d.pointLightOuterAngle = tweenAngle.value;
            if (light2d.pointLightOuterAngle < tweenAngle.startValue) light2d.pointLightOuterAngle = tweenAngle.startValue;
        }
    }
}
