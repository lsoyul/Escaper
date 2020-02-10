using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Playables;

public class CutSceneControl : MonoBehaviour
{
    public PlayableDirector playableDirector;

    public GameStatics.SCENE_INDEX nextTargetScene;
    bool isFinish = false;
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
    }
}
