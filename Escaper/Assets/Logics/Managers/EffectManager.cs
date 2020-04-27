using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;
    private static GameObject container;
    public static EffectManager GetInstance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "EffectManager";
            instance = container.AddComponent(typeof(EffectManager)) as EffectManager;

            LoadResources();
        }

        return instance;
        
    }

    static void LoadResources()
    {
        Resources.Load("Effects/Eff_jumpsmoke");
        Resources.Load("Effects/Eff_jumptwice");
        Resources.Load("Effects/Eff_MagicPillarBlastYellow");
    }


    public void playEffect(Vector3 targetPos, GameStatics.EFFECT effect, Vector2 targetVec, bool xReverse = false, Transform parent = null)
    {
        GameObject go_eff;
        switch (effect)
        {
            case GameStatics.EFFECT.JUMP_SMOKE:
                go_eff = Instantiate(Resources.Load("Effects/Eff_jumpsmoke"), targetPos, Quaternion.Euler(0, 0, 0)) as GameObject;

                if (xReverse)
                {
                    Vector3 fixedScale = go_eff.transform.localScale;
                    fixedScale.x *= -1;
                    go_eff.transform.localScale = fixedScale;
                }
                if (parent != null) 
                {
                    go_eff.transform.parent = parent;
                    go_eff.transform.localPosition = targetPos;
                }
            break;

            case GameStatics.EFFECT.JUMP_TWICE:
                go_eff = Instantiate(Resources.Load("Effects/Eff_jumptwice"), targetPos, Quaternion.Euler(0, 0, 0)) as GameObject;
                if (parent != null) 
                {
                    go_eff.transform.parent = parent;
                    go_eff.transform.localPosition = targetPos;
                }
            break;

            case GameStatics.EFFECT.GET_SHARD:

                break;
            case GameStatics.EFFECT.YELLOW_PILLAR:

                Vector3 fixedTargetPos = targetPos;
                fixedTargetPos.y -= 14f;

                go_eff = Instantiate(Resources.Load("Effects/Eff_MagicPillarBlastYellow"), fixedTargetPos, Quaternion.Euler(-90, 0, 0)) as GameObject;
                if (parent != null)
                {
                    go_eff.transform.parent = parent;
                    go_eff.transform.localPosition = targetPos;
                }
                break;
        }

    }

}
