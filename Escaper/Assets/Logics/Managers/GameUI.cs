using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    public Text currentHP;
    public TweenColor hpTweenColor;
    
    Color targetHPColor;

    private void OnEnable()
    {
        PlayerManager.Instance().onChangePlayerStatus += OnChangePlayerStatus;
    }

    private void OnDisable()
    {
        PlayerManager.Instance().onChangePlayerStatus -= OnChangePlayerStatus;
    }

    void OnChangePlayerStatus()
    {
        PlayHPTween(true);
    }



    void PlayHPTween(bool activeTween)
    {

        int tempCurrentHP = PlayerManager.Instance().PlayerStatus.CurrentHP;

        float hpRatio = tempCurrentHP / (float)PlayerManager.Instance().PlayerStatus.MaxHP;
        targetHPColor = Color.green;
        targetHPColor.r = 255f * (1f - hpRatio);

        currentHP.text = tempCurrentHP.ToString();

        hpTweenColor.startingColor = Color.white;
        hpTweenColor.endColor = targetHPColor;

        if (activeTween)
        {
            hpTweenColor.Begin();
            hpTweenColor.colorResults = hpTweenColor.startingColor;
        }
        else
        {
            hpTweenColor.colorResults = hpTweenColor.endColor;
        }
    }
}
