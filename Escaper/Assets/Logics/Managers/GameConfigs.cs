using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigs
{
    #region ### Configs ###

    private static int maxProgressStage = 0;
    private static int skillLevel_MaxHP = 0;
    private static int skillLevel_AirTimeDuration = 0;
    private static int skillLevel_IncreaseShardsPullDistance = 0;
    private static int currentMemoryShards = 0;

    private const int maxSkillLevel_MaxHP = 999;
    private const int maxSkillLevel_AirTimeDuration = 25;
    private const int maxSkillLevel_ShardPullDistance = 30;

    private static int portal_stage1_1 = 0;
    private static int portal_stage2_1 = 0;

    #endregion

    #region ### GETTER ###
    public static int MaxProgressStage
    {
        get { return maxProgressStage; }
    }

    public static bool PortalStatus(GameStatics.PORTAL_TYPE portalType)
    {
        switch (portalType)
        {
            case GameStatics.PORTAL_TYPE.STAGE1_1:
                return (portal_stage1_1 == 0) ? false : true;
            case GameStatics.PORTAL_TYPE.STAGE2_1:
                return (portal_stage2_1 == 0) ? false : true;
            default:
                return false;
        }
    }

    public static int SkillLevel(GameStatics.SKILL_TYPE skillType)
    {
        switch (skillType)
        {
            case GameStatics.SKILL_TYPE.MAXHP:
                return skillLevel_MaxHP;
            case GameStatics.SKILL_TYPE.AIRTIME_DURATION:
                return skillLevel_AirTimeDuration;
            case GameStatics.SKILL_TYPE.SHARD_PULL_DIST:
                return skillLevel_IncreaseShardsPullDistance;
            default:
                return 0;
        }
    }

    public static int MAXSkillLevel(GameStatics.SKILL_TYPE skillType)
    {
        switch (skillType)
        {
            case GameStatics.SKILL_TYPE.MAXHP:
                return maxSkillLevel_MaxHP;
            case GameStatics.SKILL_TYPE.AIRTIME_DURATION:
                return maxSkillLevel_AirTimeDuration;
            case GameStatics.SKILL_TYPE.SHARD_PULL_DIST:
                return maxSkillLevel_ShardPullDistance;
            default:
                return 0;
        }
    }

    public static int CurrentMemoryShards
    {
        get { return currentMemoryShards; }
    }
    #endregion


    public static void LoadConfigs()
    {
        maxProgressStage = PlayerPrefs.GetInt(GameStatics.PREFS_MaxProgressStage, 0);
        skillLevel_MaxHP = PlayerPrefs.GetInt(GameStatics.PREFS_SkillLevel_MaxHP, 0);
        skillLevel_AirTimeDuration = PlayerPrefs.GetInt(GameStatics.PREFS_SkillLevel_AirTimeDuration, 0);
        skillLevel_IncreaseShardsPullDistance = PlayerPrefs.GetInt(GameStatics.PREFS_SkillLevel_IncreaseShardsPullDistance, 0);
        currentMemoryShards = PlayerPrefs.GetInt(GameStatics.PREFS_CurrentMemoryShards, 0);

        portal_stage1_1 = PlayerPrefs.GetInt(GameStatics.PREFS_PORTAL_Stage1_1, 0);
        portal_stage2_1 = PlayerPrefs.GetInt(GameStatics.PREFS_PORTAL_Stage2_1, 0);
    }

    public static void InitializeConfigs()
    {
        maxProgressStage = 0;
        skillLevel_MaxHP = 0;
        skillLevel_AirTimeDuration = 0;
        skillLevel_IncreaseShardsPullDistance = 0;
        currentMemoryShards = 0;

        PlayerPrefs.SetInt(GameStatics.PREFS_MaxProgressStage, maxProgressStage);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_MaxHP, skillLevel_MaxHP);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_AirTimeDuration, skillLevel_AirTimeDuration);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_IncreaseShardsPullDistance, skillLevel_IncreaseShardsPullDistance);
        PlayerPrefs.SetInt(GameStatics.PREFS_CurrentMemoryShards, currentMemoryShards);

        PlayerPrefs.SetInt(GameStatics.PREFS_PORTAL_Stage1_1, 0);
    }

    public static void SetMaxProgressStage(int maxStage)
    {
        if (maxStage > maxProgressStage)
        {
            maxProgressStage = maxStage;
            PlayerPrefs.SetInt(GameStatics.PREFS_MaxProgressStage, maxProgressStage);
        }
    }


    public static void SetSkillLevel(GameStatics.SKILL_TYPE skillType, int level)
    {
        switch (skillType)
        {
            case GameStatics.SKILL_TYPE.MAXHP:
                skillLevel_MaxHP = level;
                PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_MaxHP, skillLevel_MaxHP);
                break;
            case GameStatics.SKILL_TYPE.AIRTIME_DURATION:
                skillLevel_AirTimeDuration = level;
                PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_AirTimeDuration, skillLevel_AirTimeDuration);
                break;
            case GameStatics.SKILL_TYPE.SHARD_PULL_DIST:
                skillLevel_IncreaseShardsPullDistance = level;
                PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_IncreaseShardsPullDistance, skillLevel_IncreaseShardsPullDistance);
                break;
            default:
                break;
        }
    }

    public static void SetCurrentMemoryShards(int memoryShardsAmount)
    {
        currentMemoryShards = memoryShardsAmount;
        PlayerPrefs.SetInt(GameStatics.PREFS_CurrentMemoryShards, currentMemoryShards);
    }

    public static void SetPortalStatus(GameStatics.PORTAL_TYPE portalType, bool isOn)
    {
        int isOnInt = (isOn) ? 1 : 0;

        switch (portalType)
        {
            case GameStatics.PORTAL_TYPE.STAGE1_1:
                PlayerPrefs.SetInt(GameStatics.PREFS_PORTAL_Stage1_1, isOnInt);
                break;
            case GameStatics.PORTAL_TYPE.STAGE2_1:
                PlayerPrefs.SetInt(GameStatics.PREFS_PORTAL_Stage2_1, isOnInt);
                break;
            default:
                break;
        }
    }
}
