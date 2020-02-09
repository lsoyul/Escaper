using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatics
{
    public const float fixedDeltaOffset = 0.1f;

    public enum GAME_STATUS
    {
        NONE = 0,
        SPLASH = 1,
        MAINMENU_ANIMATING = 2,
        MAINMENU = 3,
        OPENING = 4,
        STAGE = 5,
        GAMEOVER = 6,
    }

    public enum SCENE_INDEX
    {
        LOADING = 0,
        MAINMENU = 1,
        GAMING = 2
    }

    public enum EFFECT
    {
        JUMP_SMOKE,
        JUMP_TWICE
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
}
