using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class UpgradeElement : MonoBehaviour
{
    public GameStatics.SKILL_TYPE skillType;

    public Image imgIcon;
    public Text level;
    public Text desc;
    public Text expect;
    public Text cost;

    public GameStatics.UPGRADE_STATUS upgradeStatus;

    public System.Action<GameStatics.SKILL_TYPE> onClickUpgradeButton;

    public GameStatics.UPGRADE_STATUS GetUpgradeStatus()
    {
        return upgradeStatus;
    }

    public void SetInfo()
    {
        level.text = string.Format("LV.{0}", GameConfigs.SkillLevel(skillType));
        cost.text = GameStatics.GetRequiredShardsForUpgrade(skillType).ToString();
        switch (skillType)
        {
            case GameStatics.SKILL_TYPE.MAXHP:

                int currentHPAmount = PlayerManager.Instance().PlayerStatus.MaxHP;
                expect.text = string.Format("{0} > {1}", currentHPAmount, currentHPAmount + (GameStatics.default_IncHP));

                break;
            case GameStatics.SKILL_TYPE.AIRTIME_DURATION:

                double currentDJCoolAmount = PlayerManager.Instance().PlayerStatus.AirTimeDuration;
                expect.text = string.Format("{0:0.0} > {1:0.0}", currentDJCoolAmount, currentDJCoolAmount + (GameStatics.default_IncAirTimeDuration));

                break;
            case GameStatics.SKILL_TYPE.SHARD_PULL_DIST:

                double currentSPDist = GameStatics.GetShardPullDistance();
                expect.text = string.Format("{0:0.0} > {1:0.0}", currentSPDist, currentSPDist + (GameStatics.default_IncShardsPullDistance));

                break;
            default:
                break;
        }

        SetUpgradeStatus();

    }

    public void OnClickUpgradeButton()
    {
        if (upgradeStatus == GameStatics.UPGRADE_STATUS.POSSIBLE)
        {
            if (onClickUpgradeButton != null) onClickUpgradeButton(skillType);
        }
    }

    public void SetUpgradeStatus()
    {
        if (GameConfigs.SkillLevel(this.skillType) >= GameConfigs.MAXSkillLevel(this.skillType))
        {
            level.text = "LV.MAX";

            Color tColor = cost.color;
            tColor.a = 0.4f;
            cost.color = tColor;

            upgradeStatus = GameStatics.UPGRADE_STATUS.MAX_LEVEL;

            switch (this.skillType)
            {
                case GameStatics.SKILL_TYPE.MAXHP:
                    expect.text = PlayerManager.Instance().PlayerStatus.MaxHP.ToString();
                    break;
                case GameStatics.SKILL_TYPE.AIRTIME_DURATION:
                    expect.text = PlayerManager.Instance().PlayerStatus.AirTimeDuration.ToString();
                    break;
                case GameStatics.SKILL_TYPE.SHARD_PULL_DIST:
                    expect.text = GameStatics.GetShardPullDistance().ToString();
                    break;
                default:
                    break;
            }
        }
        else if (GameStatics.GetRequiredShardsForUpgrade(skillType) > PlayerManager.Instance().PlayerStatus.CurrentMemoryShards)
        {
            // 조각 부족
            Color tColor = cost.color;
            tColor.a = 0.4f;
            cost.color = tColor;

            upgradeStatus = GameStatics.UPGRADE_STATUS.NOT_ENOUGH_SHARD;
        }
        else
        {
            // 업그레이드 가능
            Color tColor = cost.color;
            tColor.a = 1f;
            cost.color = tColor;

            upgradeStatus = GameStatics.UPGRADE_STATUS.POSSIBLE;
        }
    }
}
