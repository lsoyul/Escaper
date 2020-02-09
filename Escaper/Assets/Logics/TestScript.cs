using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text sceneString;
    public Text levelString;

    public void OnClickChangeScene()
    {
        int sceneNum = int.Parse(sceneString.text);

        if (sceneNum >= 0 && sceneNum <= 3) TopMostControl.Instance().StartChangeScene((GameStatics.SCENE_INDEX)sceneNum, true);
    }

    public void OnClickChangeLevel()
    {
        int levelNum = int.Parse(levelString.text);

        if (levelNum >= 0 && levelNum <= StageLoader.Instance().stageList.Count) 
        {
            StageLoader.Instance().SetStage(levelNum);
        }
    }
}
