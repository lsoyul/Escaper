using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigs
{
    #region ### Configs ###

    private static int maxProgressStage = 0;
    private static int skillLevel_MaxHP = 0;
    private static int skillLevel_DoubleJumpCooltime = 0;
    private static int skillLevel_IncreaseShardsPullDistance = 0;
    private static int currentMemoryShards = 0;

    #endregion

    #region ### GETTER ###
    public static int MaxProgressStage
    {
        get { return maxProgressStage; }
    }

    public static int SkillLevel_MaxHP
    {
        get { return skillLevel_MaxHP; }
    }

    public static int SkillLevel_DoubleJumpCooltime
    {
        get { return skillLevel_DoubleJumpCooltime; }
    }

    public static int SkillLevel_IncreaseShardsPullDistance
    {
        get { return skillLevel_IncreaseShardsPullDistance; }
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
        skillLevel_DoubleJumpCooltime = PlayerPrefs.GetInt(GameStatics.PREFS_SkillLevel_DoubleJumpCooltime, 0);
        skillLevel_IncreaseShardsPullDistance = PlayerPrefs.GetInt(GameStatics.PREFS_SkillLevel_IncreaseShardsPullDistance, 0);
        currentMemoryShards = PlayerPrefs.GetInt(GameStatics.PREFS_CurrentMemoryShards, 0);
    }

    public static void InitializeConfigs()
    {
        maxProgressStage = 0;
        skillLevel_MaxHP = 0;
        skillLevel_DoubleJumpCooltime = 0;
        skillLevel_IncreaseShardsPullDistance = 0;
        currentMemoryShards = 0;

        PlayerPrefs.SetInt(GameStatics.PREFS_MaxProgressStage, maxProgressStage);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_MaxHP, skillLevel_MaxHP);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_DoubleJumpCooltime, skillLevel_DoubleJumpCooltime);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_IncreaseShardsPullDistance, skillLevel_IncreaseShardsPullDistance);
        PlayerPrefs.SetInt(GameStatics.PREFS_CurrentMemoryShards, currentMemoryShards);
    }

    public static void SetMaxProgressStage(int maxStage)
    {
        if (maxStage > maxProgressStage)
        {
            maxProgressStage = maxStage;
            PlayerPrefs.SetInt(GameStatics.PREFS_MaxProgressStage, maxProgressStage);
        }
    }

    public static void SetSkillLevel_MaxHP(int level)
    {
        skillLevel_MaxHP = level;
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_MaxHP, skillLevel_MaxHP);
    }
    public static void SetSkillLevel_DoubleJumpCooltime(int level)
    {
        skillLevel_DoubleJumpCooltime = level;
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_DoubleJumpCooltime, skillLevel_DoubleJumpCooltime);
    }
    public static void SetSkillLevel_IncreaseShardsPullDistance(int level)
    {
        skillLevel_IncreaseShardsPullDistance = level;
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_IncreaseShardsPullDistance, skillLevel_IncreaseShardsPullDistance);
    }

    public static void SetCurrentMemoryShards(int memoryShardsAmount)
    {
        currentMemoryShards = memoryShardsAmount;
        PlayerPrefs.SetInt(GameStatics.PREFS_CurrentMemoryShards, currentMemoryShards);
    }
}
