using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CodeStage.AntiCheat.ObscuredTypes;

public static class GameStatics
{
    public static ObscuredFloat fixedDeltaOffset = 0.1f;

    #region ### Default Status ###

    public static ObscuredInt default_IncHP = 5;
    public static ObscuredDouble default_IncAirTimeDuration = 0.1f;
    public static ObscuredDouble default_IncShardsPullDistance = 2f;
    public static ObscuredInt default_BaseMaxHP = 40;
    public static ObscuredDouble default_AirTimeDuration = 1f;
    public static ObscuredDouble default_ShardsPullDistance = 10.0f;
    // Ads sale ratio
    public static ObscuredDouble default_MultiflyRatio = 1.5f;
    public static ObscuredInt default_MaxReviceCount = 1;

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
        YELLOW_PILLAR,
        Explosion1,
        CONFETTI_BLAST,
        CONFETTI_DIRECTIONAL
    }

    public enum DAMAGED_TYPE
    {
        SPIKE,
        PROJECTILE_SHOOTER1,
        PROJECTILE_SHOOTER2,
        EARTH_QUAKE,
        FALLING_GROUND
    }

    public enum PROJECTILE_TYPE
    {
        SHOOTER1,
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
        STAGE3_1,
    }

    public enum PLAY_MODE
    {
        NORMAL,
        TRUE
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
                const int default_IncCoolShards = 30;
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

    public static ObscuredInt Shard1_min = 7;
    public static ObscuredInt Shard1_max = 14;
    public static ObscuredInt Shard2_min = 80;
    public static ObscuredInt Shard2_max = 101;

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

    private static ObscuredInt DAMAGE_BASIC_SPIKE = 10;
    private static ObscuredInt DAMAGE_PROJECTILE1 = 15;
    private static ObscuredInt DAMAGE_PROJECTIlE2 = 20;

    public static int GetDamagePoints(DAMAGED_TYPE damageType)
    {
        switch (damageType)
        {
            case DAMAGED_TYPE.SPIKE:
                return DAMAGE_BASIC_SPIKE;
            case DAMAGED_TYPE.PROJECTILE_SHOOTER1:
                return DAMAGE_PROJECTILE1;
            case DAMAGED_TYPE.PROJECTILE_SHOOTER2:
                return DAMAGE_PROJECTIlE2;
            case DAMAGED_TYPE.EARTH_QUAKE:
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
    public const string sound_trapFire1 = "sound_trapFire1";
    public const string sound_powerUp = "sound_powerUp";
    public const string sound_victory = "sound_victory";
    public const string sound_explosion1 = "sound_explosion1";

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
                return 1000;
            case PORTAL_TYPE.STAGE2_1:
                return 2000;
            case PORTAL_TYPE.STAGE3_1:
                return 3000;
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
            case 3:
                return -51f;
            default:
                return 0;
        }
    }

    #endregion

    #region #### Animation Sprites name ####

    public enum PLAYER_SKIN
    {
        NORMAL,
        HERO_MINT,
        HERO_8BIT,
        HERO_COLD,
        HERO_INSANE,
        HERO_GRAY,
        HERO_INVIS,
    }

    public static string GetPlayerSpriteName(PLAYER_SKIN playerSprite)
    {
        switch (playerSprite)
        {
            case PLAYER_SKIN.NORMAL:
                return "Hero_Sheet";
            case PLAYER_SKIN.HERO_MINT:
                return "Hero_Sheet_mint";
            case PLAYER_SKIN.HERO_8BIT:
                return "Hero_Sheet_8bit";
            case PLAYER_SKIN.HERO_COLD:
                return "Hero_Sheet_cold";
            case PLAYER_SKIN.HERO_INSANE:
                return "Hero_Sheet_insane";
            case PLAYER_SKIN.HERO_GRAY:
                return "Hero_Sheet_gray";
            case PLAYER_SKIN.HERO_INVIS:
                return "Hero_Sheet_invis";
            default:
                return "Hero_Sheet";
        }
    }


    #endregion

    #region #### Log Events ####

    public const string EVENT_START_GAME_NORMAL = "E_StartGame_Normal";
    public const string EVENT_START_GAME_TRUEHERO = "E_StartGame_TrueHero";
    public const string EVENT_CLEAR_STAGE1 = "E_Clear_Stage1";
    public const string EVENT_CLEAR_STAGE2 = "E_Clear_Stage2";
    public const string EVENT_TRIGGER_ENDING_NORMAL = "E_Trigger_Ending_Normal";
    public const string EVENT_TRIGGER_ENDING_TRUEHERO = "E_Trigger_Ending_TrueHero";
    public const string EVENT_OPEN_PORTAL_STAGE1 = "E_Open_Portal_Stage1";
    public const string EVENT_OPEN_PORTAL_STAGE2 = "E_Open_Portal_Stage2";
    public const string EVENT_OPEN_PORTAL_STAGE3 = "E_Open_Portal_Stage3";
    public const string EVENT_REVIVE_SHARD = "E_Revive_Shard";
    public const string EVENT_REVIVE_ADS = "E_Revive_Ads";

    #endregion

    #region #### PlayerPrefs Keys ####

    public const string PREFS_MaxProgressStage = "PREFS_MaxProgressStage";
    public const string PREFS_SkillLevel_MaxHP = "PREFS_SkillLevel_MaxHP";
    public const string PREFS_SkillLevel_AirTimeDuration = "PREFS_SkillLevel_AirTimeDuration";
    public const string PREFS_SkillLevel_IncreaseShardsPullDistance = "PREFS_SkillLevel_IncreaseShardsPullDistance";
    public const string PREFS_CurrentMemoryShards = "PREFS_CurrentMemoryShards";

    public const string PREFS_PORTAL_Stage1_1 = "PREFS_PORTAL_Stage1_1";
    public const string PREFS_PORTAL_Stage2_1 = "PREFS_PORTAL_Stage2_1";
    public const string PREFS_PORTAL_Stage3_1 = "PREFS_PORTAL_Stage3_1";

    public const string PREFS_VOLUME_BGM = "PREFS_VOLUME_BGM";
    public const string PREFS_VOLUME_SFX = "PREFS_VOLUME_SFX";

    public const string PREFS_VIBRATE = "PREFS_VIBRATE";

    public const string PREFS_PLAYTIME = "PREFS_PLAYTIME";

    public const string PREFS_ENDING_NORMAL = "PREFS_ENDING_NORMAL";
    public const string PREFS_ENDING_TRUE = "PREFS_ENDING_TRUE";

    public const string PREFS_LAST_PLAYMODE = "PREFS_LAST_PLAYMODE";
    public const string PREFS_WATCHED_TUTORIAL = "PREFS_WATCHED_TUTORIAL";
    #endregion
}
