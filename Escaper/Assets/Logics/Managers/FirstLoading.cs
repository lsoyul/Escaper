using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using static GameStatics;

public class FirstLoading : MonoBehaviour
{
    public TweenAlpha splashImageTween;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        splashImageTween.TweenCompleted += () => {
            TopMostControl.Instance().StartChangeScene(SCENE_INDEX.MAINMENU, true);
        };

        splashImageTween.Begin();
    }
}
