using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class simpleMainTitle : MonoBehaviour
{
    Text maintitle;
    TweenColor titleTweenColor;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        maintitle.color = titleTweenColor.colorResults;
    }
}
