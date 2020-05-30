using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text hpSkillLevelString;
    public Text coolSkilllevelString;
    public Text distanceSkillLevelString;
    public Text stageString;

    public void OnClickMaxHP()
    {
        PlayerManager.Instance().PlayerStatus.CurrentHP = PlayerManager.Instance().PlayerStatus.MaxHP;
    }

    public void OnClickIncreaseShard()
    {
        PlayerManager.Instance().PlayerStatus.CurrentMemoryShards += 30;
    }

    public void OnClickHPLevel()
    {
        GameConfigs.SetSkillLevel(GameStatics.SKILL_TYPE.MAXHP, int.Parse(hpSkillLevelString.text));
    }
    public void OnClickSkillCoolLevel()
    {
        GameConfigs.SetSkillLevel(GameStatics.SKILL_TYPE.AIRTIME_DURATION,int.Parse(coolSkilllevelString.text));
    }

    public void OnClickSkillDistanceLevel()
    {
        GameConfigs.SetSkillLevel(GameStatics.SKILL_TYPE.SHARD_PULL_DIST,int.Parse(distanceSkillLevelString.text));
    }

    public void OnClickPortalOn()
    {
        GameConfigs.SetPortalStatus(GameStatics.PORTAL_TYPE.STAGE1_1, true);
        GameConfigs.SetPortalStatus(GameStatics.PORTAL_TYPE.STAGE2_1, true);
        GameConfigs.SetPortalStatus(GameStatics.PORTAL_TYPE.STAGE3_1, true);
    }


    public void OnClickPortalOff()
    {
        GameConfigs.SetPortalStatus(GameStatics.PORTAL_TYPE.STAGE1_1, false);
        GameConfigs.SetPortalStatus(GameStatics.PORTAL_TYPE.STAGE2_1, false);
        GameConfigs.SetPortalStatus(GameStatics.PORTAL_TYPE.STAGE3_1, false);
    }

    public void OnClickSetStage()
    {
        TopMostControl.Instance().StartChangeScene(GameStatics.SCENE_INDEX.GAMESTAGE, true, int.Parse(stageString.text));
    }

    public void OnClickChangeRandomSkin()
    {
        PlayerManager.Instance().TEST_SetRandomSkin();
    }

}
