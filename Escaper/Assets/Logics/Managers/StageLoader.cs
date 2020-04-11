using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public List<GameObject> stageList;
    private static GameObject container;
    private static StageLoader instance;

    public static int CurrentStage = 1;
    public static int NextStage = 1;

    public static StageLoader Instance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "StageLoader";
            instance = container.AddComponent(typeof(StageLoader)) as StageLoader;
        }

        return instance;
    }

    private void Awake() {
        
        if (instance == null) 
        {
            instance = GetComponent<StageLoader>();
            DontDestroyOnLoad(this);
        }
    }

    public void SetStage(int targetStage)
    {
        int tarStage = targetStage;
        if (tarStage > stageList.Count)
        {
            tarStage = 1;
        }

        CurrentStage = tarStage;
        PlayerManager.Instance().InitializePlayer();

        int stage = tarStage - 1;
        
        for (int i = 0; i < stageList.Count; i++)
        {
            if (stage == i) stageList[i].SetActive(true);
            else stageList[i].SetActive(false);
        }
    }

    public void DisableStage()
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            stageList[i].SetActive(false);
        }
    }
}
