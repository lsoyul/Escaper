using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Playables;

public class CutSceneControl : MonoBehaviour
{
    public PlayableDirector playableDirector;

    public GameStatics.SCENE_INDEX nextTargetScene;
    bool isFinish = false;

    private Touch tempTouchs;

    public TweenAlpha touchGuideTweenAlpha;
    private bool touchOn = false;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (isFinish == false)
        {
            if (playableDirector.state != PlayState.Playing)
            {
                isFinish = true;
                TopMostControl.Instance().StartChangeScene(nextTargetScene, true);
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (touchOn == false)
            {
                touchOn = true;
                touchGuideTweenAlpha.Begin();
                touchGuideTweenAlpha.value = touchGuideTweenAlpha.startValue;

            }
            else
            {
                if (isFinish == false)
                {
                    isFinish = true;
                    TopMostControl.Instance().StartChangeScene(nextTargetScene, true);
                }
            }
        }

#else
        if (Input.touchCount > 0)
        {    
            for (int i = 0; i < Input.touchCount; i++)
            {
                tempTouchs = Input.GetTouch(i);
                if (tempTouchs.phase == TouchPhase.Began)
                {
                    if (touchOn == false) { 
                        touchOn = true;
                        touchGuideTweenAlpha.Begin();
                        touchGuideTweenAlpha.value = touchGuideTweenAlpha.startValue;

                    }
                    else
                    {
                        if (isFinish == false)
                        {
                            isFinish = true;
                            TopMostControl.Instance().StartChangeScene(nextTargetScene, true);
                        }
                    }

                    break;   
                }
            }
        }
#endif

    }
}
