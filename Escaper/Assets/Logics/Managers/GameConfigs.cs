using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigs
{
    #region ### Configs ###

    private static int maxProgressStage = 0;
    private static int skillLevel_MaxHP = 0;
    private static int skillLevel_DoubleJumpCooltime = 0;
    private static int skillLevel_AirTimeDuration = 0;
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

    public static int SkillLevel_AirTimeDuration
    {
        get { return skillLevel_AirTimeDuration; }
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
        skillLevel_AirTimeDuration = PlayerPrefs.GetInt(GameStatics.PREFS_SkillLevel_AirTimeDuration, 0);
        currentMemoryShards = PlayerPrefs.GetInt(GameStatics.PREFS_CurrentMemoryShards, 0);
    }

    public static void InitializeConfigs()
    {
        maxProgressStage = 0;
        skillLevel_MaxHP = 0;
        skillLevel_DoubleJumpCooltime = 0;
        skillLevel_AirTimeDuration = 0;
        currentMemoryShards = 0;

        PlayerPrefs.SetInt(GameStatics.PREFS_MaxProgressStage, maxProgressStage);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_MaxHP, skillLevel_MaxHP);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_DoubleJumpCooltime, skillLevel_DoubleJumpCooltime);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_AirTimeDuration, skillLevel_AirTimeDuration);
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
    public static void SetSkillLevel_AirTimeDuration(int level)
    {
        skillLevel_AirTimeDuration = level;
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_AirTimeDuration, skillLevel_AirTimeDuration);
    }

    public static void SetCurrentMemoryShards(int memoryShardsAmount)
    {
        currentMemoryShards = memoryShardsAmount;
        PlayerPrefs.SetInt(GameStatics.PREFS_CurrentMemoryShards, currentMemoryShards);
    }
}
