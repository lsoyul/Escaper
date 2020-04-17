using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public List<StageBase> stageList;
    private static GameObject container;
    private static StageLoader instance;

    public static int CurrentStage = 1;
    public static int NextStage = 1;

    // Shard Control
    [Header("- Shard Controls -")]
    public GameObject shard1_GO;
    public Transform generatedShardRoot;
    bool isShardMultiflyMode = false;

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
        PlayerManager.Instance().SetPlayerController(true);

        int stage = tarStage - 1;
        
        for (int i = 0; i < stageList.Count; i++)
        {
            if (stage == i) stageList[i].gameObject.SetActive(true);
            else stageList[i].gameObject.SetActive(false);
        }

        // Generate Shards
        ClearShards();
        GenerateShards(CurrentStage - 1);
    }

    public void DisableStage()
    {
        ClearShards();
        for (int i = 0; i < stageList.Count; i++)
        {
            stageList[i].gameObject.SetActive(false);
        }

        PlayerManager.Instance().SetPlayerController(false);
    }

    #region SHARD Calculates

    public float GetShardMultifly()
    {
        double multiflyRatio = 1f;
        if (isShardMultiflyMode) multiflyRatio = GameStatics.default_MultiflyRatio;

        switch (CurrentStage)
        {
            case 1:
                return CurrentStage * (float)multiflyRatio;
            case 2:
                return CurrentStage * (float)multiflyRatio;
            case 3:
                return CurrentStage * (float)multiflyRatio;
            default:
                return 0;
        }
    }

    public int GetRandomShard(GameStatics.SHARD_TYPE shardType)
    {
        switch (shardType)
        {
            case GameStatics.SHARD_TYPE.SHARD1:
                return Random.Range(GameStatics.Shard1_min, GameStatics.Shard1_max);
            case GameStatics.SHARD_TYPE.SHARD2:
                return Random.Range(GameStatics.Shard2_min, GameStatics.Shard2_max);
            default:
                return 0;
        }
    }

    void GenerateShards(int stageIndex)
    {
        List<Transform> posList = stageList[stageIndex].ShardPosList();

        foreach (Transform targetPos in posList)
        {
            GameObject shard = Instantiate(shard1_GO, generatedShardRoot);

            MemoryShard ms = shard.GetComponent<MemoryShard>();

            if (ms != null)
            {
                ms.SetShard(
                    GameStatics.SHARD_TYPE.SHARD1, 
                    targetPos.position, 
                    PlayerManager.Instance().GetPlayerControl().GetPlayerRigidBody().transform);

                ms.onGetShard = PlayerManager.Instance().OnGetShard;
            }
        }
    }

    public StageBase GetCurrentStage()
    {
        return stageList[CurrentStage - 1];
    }

    void ClearShards()
    {
        foreach (Transform tran in generatedShardRoot)
        {
            Destroy(tran.gameObject);
        }
    }

    #endregion

}
