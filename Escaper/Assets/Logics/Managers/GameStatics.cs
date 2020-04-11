using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatics
{
    public const float fixedDeltaOffset = 0.1f;

    #region ### Default Status ###

    public const int default_IncHP = 5;
    public const double default_IncDoublejumpCooltime = -0.1f;
    public const double default_IncAirTimeDuration = 0.2f;
    public const int default_BaseMaxHP = 40;
    public const double default_DoubleJumpCooltime = 5.0f;
    public const double default_AirtimeDuration = 1.0f;
    public const int default_IncMemoryShards = 10;
    // Ads sale ratio
    public const double default_SaleBonusRatio = 0.8f;
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
        JUMP_TWICE
    }

    public enum DAMAGED_TYPE
    {
        SPIKE,
        
    }

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

    public const int DMG_SPIKE_BASIC = 20;

    #endregion


    #region #### Log Events ####

    public const string EVENT_LOGIN_ANONYMOUS = "E_Signin_Anony";
    public const string EVENT_LOGIN_GOOGLE = "E_Login_Google";
    public const string EVENT_LOGOUT = "E_Logout";

    #endregion

    #region #### PlayerPrefs Keys ####

    public const string PREFS_MaxProgressStage = "PREFS_MaxProgressStage";
    public const string PREFS_SkillLevel_MaxHP = "PREFS_SkillLevel_MaxHP";
    public const string PREFS_SkillLevel_DoubleJumpCooltime = "PREFS_SkillLevel_DoubleJumpCooltime";
    public const string PREFS_SkillLevel_AirTimeDuration = "PREFS_SkillLevel_AirTimeDuration";
    public const string PREFS_CurrentMemoryShards = "PREFS_CurrentMemoryShards";


    #endregion
}
