using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text coolSkilllevelString;
    public Text distanceSkillLevelString;

    public void OnClickMaxHP()
    {
        PlayerManager.Instance().PlayerStatus.CurrentHP = PlayerManager.Instance().PlayerStatus.MaxHP;
    }

    public void OnClickSkillCoolLevel()
    {
        GameConfigs.SetSkillLevel_DoubleJumpCooltime(int.Parse(coolSkilllevelString.text));
    }

    public void OnClickSkillDistanceLevel()
    {
        GameConfigs.SetSkillLevel_IncreaseShardsPullDistance(int.Parse(distanceSkillLevelString.text));
    }
}
