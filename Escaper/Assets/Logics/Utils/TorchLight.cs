using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;

public class TorchLight : MonoBehaviour
{
    public Light2D torch;
    public TweenValue tweenValue;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        torch.pointLightOuterRadius = tweenValue.value;
    }
}
