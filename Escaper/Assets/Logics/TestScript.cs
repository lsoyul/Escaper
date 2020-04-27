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
}
