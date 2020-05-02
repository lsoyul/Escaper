using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatics
{
    public const float fixedDeltaOffset = 0.1f;

    #region ### Default Status ###

    public const int default_IncHP = 5;
    public const double default_IncAirTimeDuration = 0.1f;
    public const double default_IncShardsPullDistance = 1f;
    public const int default_BaseMaxHP = 40;
    public const double default_AirTimeDuration = 1f;
    public const double default_ShardsPullDistance = 10.0f;
    // Ads sale ratio
    public const double default_MultiflyRatio = 1.5f;
    public const int default_MaxReviceCount = 1;

    #endregion

    public enum LOGIN_TYPE
    {
        FAIL,
        ANONYMOUS,
        GOOGLE
    }

    public enum GAME_STATUS
    {
        NONE,
        SPLASH,
        MAINMENU,
        CUTSCENE,
        STAGE,
        STAGE_PAUSE,
        GAMEOVER,
    }

    public enum SCENE_INDEX
    {
        LOADING = 0,
        MAINMENU = 1,
        CUTSCENE = 2,
        GAMESTAGE = 3
    }

    public enum EFFECT
    {
        JUMP_SMOKE,
        JUMP_TWICE,
        GET_SHARD,
        YELLOW_PILLAR
    }

    public enum DAMAGED_TYPE
    {
        SPIKE,
        FALLING_GROUND
    }

    public enum SHARD_TYPE
    {
        EFFECT,
        SHARD1,
        SHARD2
    }

    public enum SHARD_MODE
    {
        STAGE1,
        STAGE2,
        STAGE3
    }

    public enum SKILL_TYPE
    {
        MAXHP,
        AIRTIME_DURATION,
        SHARD_PULL_DIST
    }

    public enum UPGRADE_STATUS
    {
        POSSIBLE,
        NOT_ENOUGH_SHARD,
        MAX_LEVEL
    }

    public enum TOPUI_STATUS
    {
        NORMAL,
        GAMEOVER,
        GAMEOVER_SELF
    }

    public enum PORTAL_TYPE
    {
        NONE,
        STAGE1_1,
        STAGE2_1,
    }

    #region #### Logics ####

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    public static int GetRequiredShardsForUpgrade(SKILL_TYPE skillType)
    {
        int result = int.MaxValue;

        switch (skillType)
        {
            case SKILL_TYPE.MAXHP:
                const int default_IncHPShards = 10;
                result = (GameConfigs.SkillLevel(skillType) + 1) * default_IncHPShards;
                break;
            case SKILL_TYPE.AIRTIME_DURATION:
                const int default_IncCoolShards = 50;
                result = (GameConfigs.SkillLevel(skillType) + 1) * default_IncCoolShards;
                break;
            case SKILL_TYPE.SHARD_PULL_DIST:
                const int default_IncDistShards = 40;
                result = (GameConfigs.SkillLevel(skillType) + 1) * default_IncDistShards;
                break;
            default:
                break;
        }

        return result;

    }

    public static double GetShardPullDistance()
    {
        return default_ShardsPullDistance + (default_IncShardsPullDistance * GameConfigs.SkillLevel(GameStatics.SKILL_TYPE.SHARD_PULL_DIST));
    }

    #endregion


    #region #### Shards variables ####

    public const int Shard1_min = 7;
    public const int Shard1_max = 14;
    public const int Shard2_min = 80;
    public const int Shard2_max = 101;

    #endregion

    #region #### Menus ####

    public enum MENU_GAMEOVER
    {
        MAINMENU,
        RETRY,
        REVIVE_SHARDS,
        REVIVE_AD
    }

    #endregion

    #region #### Damage Points ####

    private static int DAMAGE_BASIC_SPIKE = 10;

    public static int GetDamagePoints(DAMAGED_TYPE damageType)
    {
        switch (damageType)
        {
            case DAMAGED_TYPE.SPIKE:
                return DAMAGE_BASIC_SPIKE * StageLoader.CurrentStage;
            case DAMAGED_TYPE.FALLING_GROUND:
                return (PlayerManager.Instance().PlayerStatus.MaxHP / 4);
            default:
                return 0;
        }
    }


    #endregion

    #region #### Sound String Refers ####

    public const string sound_doubleJump = "sound_doubleJump";
    public const string sound_fallGround = "sound_fallGround";
    public const string sound_gainShard  = "sound_gainShard";
    public const string sound_hitWall    = "sound_hitWall";
    public const string sound_jump       = "sound_jump";
    public const string sound_jumpStomp  = "sound_jumpStomp";
    public const string sound_portalMove = "sound_portalMove";
    public const string sound_revive     = "sound_revive";
    public const string sound_select     = "sound_select";
    public const string sound_timestop   = "sound_timestop";
    public const string sound_upgrade = "sound_upgrade";
    #endregion

    #region #### Warp Trigger ####

    public enum MOVE_TRIGGER
    {
        MOVE_POSITION,
        MOVE_NEXTSTAGE
    }

    public static int GetPortalOpenCost(PORTAL_TYPE portalType)
    {
        switch (portalType)
        {
            case PORTAL_TYPE.STAGE1_1:
                return 2000;
            case PORTAL_TYPE.STAGE2_1:
                return 5000;
            default:
                return 0;
        }
    }

    public static float GetCameraMinimumYAxis(int stage)
    {
        switch (stage)
        {
            case 1:
                return 0;
            case 2:
                return -51f;
            default:
                return 0;
        }
    }

    #endregion

    #region #### Animation Sprites name ####

    public enum PLAYER_SPRITE
    {
        NORMAL,
        TRUE_HERO
    }


    #endregion

    #region #### Log Events ####

    public const string EVENT_START_GAME = "E_StartGame";

    public const string EVENT_LOGIN_ANONYMOUS = "E_Signin_Anony";
    public const string EVENT_LOGIN_GOOGLE = "E_Login_Google";
    public const string EVENT_LOGOUT = "E_Logout";

    #endregion

    #region #### PlayerPrefs Keys ####

    public const string PREFS_MaxProgressStage = "PREFS_MaxProgressStage";
    public const string PREFS_SkillLevel_MaxHP = "PREFS_SkillLevel_MaxHP";
    public const string PREFS_SkillLevel_AirTimeDuration = "PREFS_SkillLevel_AirTimeDuration";
    public const string PREFS_SkillLevel_IncreaseShardsPullDistance = "PREFS_SkillLevel_IncreaseShardsPullDistance";
    public const string PREFS_CurrentMemoryShards = "PREFS_CurrentMemoryShards";

    public const string PREFS_PORTAL_Stage1_1 = "PREFS_PORTAL_Stage1_1";
    public const string PREFS_PORTAL_Stage2_1 = "PREFS_PORTAL_Stage2_1";

    #endregion
}
