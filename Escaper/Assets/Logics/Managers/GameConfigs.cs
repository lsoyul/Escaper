using DigitalRuby.SoundManagerNamespace;
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
    private const int maxSkillLevel_AirTimeDuration = 50;
    private const int maxSkillLevel_ShardPullDistance = 15;

    private static int portal_stage1_1 = 0;
    private static int portal_stage2_1 = 0;
    private static int portal_stage3_1 = 0;

    private static int vibrate = 1;

    private static int playunixtime = 0;

    private static int ending_normal = 0;
    private static int ending_true = 0;

    private static int last_playmode = 0;
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
            case GameStatics.PORTAL_TYPE.STAGE3_1:
                return (portal_stage3_1 == 0) ? false : true;
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
        portal_stage3_1 = PlayerPrefs.GetInt(GameStatics.PREFS_PORTAL_Stage3_1, 0);

        SoundManager.MusicVolume = PlayerPrefs.GetFloat(GameStatics.PREFS_VOLUME_BGM, 0.5f);
        SoundManager.SoundVolume = PlayerPrefs.GetFloat(GameStatics.PREFS_VOLUME_SFX, 0.5f);

        vibrate = PlayerPrefs.GetInt(GameStatics.PREFS_VIBRATE, 1);

        playunixtime = PlayerPrefs.GetInt(GameStatics.PREFS_PLAYTIME, 0);

        ending_normal = PlayerPrefs.GetInt(GameStatics.PREFS_ENDING_NORMAL, 0);
        ending_true = PlayerPrefs.GetInt(GameStatics.PREFS_ENDING_TRUE, 0);

        last_playmode = PlayerPrefs.GetInt(GameStatics.PREFS_LAST_PLAYMODE, 0);
    }

    public static void InitializeConfigs()
    {
        maxProgressStage = 0;
        skillLevel_MaxHP = 0;
        skillLevel_AirTimeDuration = 0;
        skillLevel_IncreaseShardsPullDistance = 0;
        currentMemoryShards = 0;
        vibrate = 1;
        playunixtime = 0;
        ending_normal = 0;
        ending_true = 0;
        last_playmode = 0;

        PlayerPrefs.SetInt(GameStatics.PREFS_MaxProgressStage, maxProgressStage);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_MaxHP, skillLevel_MaxHP);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_AirTimeDuration, skillLevel_AirTimeDuration);
        PlayerPrefs.SetInt(GameStatics.PREFS_SkillLevel_IncreaseShardsPullDistance, skillLevel_IncreaseShardsPullDistance);
        PlayerPrefs.SetInt(GameStatics.PREFS_CurrentMemoryShards, currentMemoryShards);

        PlayerPrefs.SetInt(GameStatics.PREFS_PORTAL_Stage1_1, 0);
        PlayerPrefs.SetInt(GameStatics.PREFS_PORTAL_Stage2_1, 0);
        PlayerPrefs.SetInt(GameStatics.PREFS_PORTAL_Stage3_1, 0);

        PlayerPrefs.SetFloat(GameStatics.PREFS_VOLUME_BGM, 0.5f);
        PlayerPrefs.SetFloat(GameStatics.PREFS_VOLUME_SFX, 0.5f);

        SoundManager.MusicVolume = 0.5f;
        SoundManager.SoundVolume = 0.5f;

        PlayerPrefs.SetInt(GameStatics.PREFS_VIBRATE, 1);

        PlayerPrefs.SetInt(GameStatics.PREFS_PLAYTIME, 0);

        PlayerPrefs.SetInt(GameStatics.PREFS_ENDING_NORMAL, 0);
        PlayerPrefs.SetInt(GameStatics.PREFS_ENDING_TRUE, 0);

        PlayerPrefs.SetInt(GameStatics.PREFS_LAST_PLAYMODE, 0);
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
                portal_stage1_1 = isOnInt;
                if (isOn) Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_OPEN_PORTAL_STAGE1);
                break;
            case GameStatics.PORTAL_TYPE.STAGE2_1:
                PlayerPrefs.SetInt(GameStatics.PREFS_PORTAL_Stage2_1, isOnInt);
                portal_stage2_1 = isOnInt;
                if (isOn) Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_OPEN_PORTAL_STAGE2);
                break;
            case GameStatics.PORTAL_TYPE.STAGE3_1:
                PlayerPrefs.SetInt(GameStatics.PREFS_PORTAL_Stage3_1, isOnInt);
                portal_stage3_1 = isOnInt;
                if (isOn) Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_OPEN_PORTAL_STAGE3);
                break;
            default:
                break;
        }
    }

    public static void SetVolume(float bgmVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat(GameStatics.PREFS_VOLUME_BGM, bgmVolume);
        PlayerPrefs.SetFloat(GameStatics.PREFS_VOLUME_SFX, sfxVolume);

        SoundManager.MusicVolume = bgmVolume;
        SoundManager.SoundVolume = sfxVolume;
    }

    public static bool GetIsVibrate()
    {
        return (vibrate == 1) ? true : false;
    }

    public static void SetIsVibrate(bool isVibrate)
    {
        vibrate = (isVibrate) ? 1 : 0;

        PlayerPrefs.SetInt(GameStatics.PREFS_VIBRATE, vibrate);
    }

    public static int GetPlayUnixTime()
    {
        return playunixtime;
    }

    public static void SetPlayTime(int playtime)
    {
        playunixtime = playtime;

        PlayerPrefs.SetInt(GameStatics.PREFS_PLAYTIME, playunixtime);
    }

    public static void SetNormalEnding()
    {
        ending_normal = 1;
        PlayerPrefs.SetInt(GameStatics.PREFS_ENDING_NORMAL, ending_normal);
    }
    public static void SetTrueEnding()
    {
        ending_true = 1;
        PlayerPrefs.SetInt(GameStatics.PREFS_ENDING_TRUE, ending_true);
    }

    public static bool GetNormalEnding()
    {
        return (ending_normal == 1) ? true : false;
    }

    public static bool GetTrueEnding()
    {
        return (ending_true == 1) ? true : false;
    }

    public static void SetLastPlayMode(GameStatics.PLAY_MODE playMode)
    {
        switch (playMode)
        {
            case GameStatics.PLAY_MODE.NORMAL:
                PlayerPrefs.SetInt(GameStatics.PREFS_LAST_PLAYMODE, 0);
                break;
            case GameStatics.PLAY_MODE.TRUE:
                PlayerPrefs.SetInt(GameStatics.PREFS_LAST_PLAYMODE, 1);
                break;
            default:
                break;
        }
    }

    public static GameStatics.PLAY_MODE GetLastPlayMode()
    {
        int playModeInt = PlayerPrefs.GetInt(GameStatics.PREFS_LAST_PLAYMODE, 0);

        switch (playModeInt)
        {
            case 0:
                return GameStatics.PLAY_MODE.NORMAL;
            case 1:
                return GameStatics.PLAY_MODE.TRUE;
            default:
                return GameStatics.PLAY_MODE.NORMAL;
        }
    }
}
