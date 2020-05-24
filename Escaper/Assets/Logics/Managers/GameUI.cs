using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    [Header("- HP & Shard UI Control -")]
    public GameObject HPSprite;
    public Text currentHP;
    public TweenColor hpTweenColor;
    public TweenVector hpSpriteTweenScale;
    public TweenVector hpTextTweenScale;

    public GameObject ShardSprite;
    public Text currentShard;
    public TweenColor shardTweenColor;
    public TweenVector shardSpriteTweenScale;
    public TweenVector shardTextTweenScale;

    Color targetHPColor;
    public Color targetShardColor;

    private int beforeHP = -1;
    private int beforeShard = -1;

    private void OnEnable()
    {
        PlayerManager.Instance().onChangePlayerStatus += OnChangePlayerStatus;
        PlayerManager.Instance().onTriggerEnding += OnTriggerEnding;
    }

    private void OnDisable()
    {
        PlayerManager.Instance().onChangePlayerStatus -= OnChangePlayerStatus;
        PlayerManager.Instance().onTriggerEnding -= OnTriggerEnding;
    }

    private void Start()
    {
        PlayHPTween(true);
        PlayShardTween(true);
    }

    void OnChangePlayerStatus()
    {
        // HP Tween
        if (PlayerManager.Instance().PlayerStatus.CurrentHP != beforeHP)
        {
            beforeHP = PlayerManager.Instance().PlayerStatus.CurrentHP;
            PlayHPTween(true);
        }

        // Shard Tween
        if (PlayerManager.Instance().PlayerStatus.CurrentMemoryShards != beforeShard)
        {
            beforeShard = PlayerManager.Instance().PlayerStatus.CurrentMemoryShards;
            PlayShardTween(true);
        }
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

            hpSpriteTweenScale.Begin();
            hpSpriteTweenScale.vector3Results = hpSpriteTweenScale.startingVector;
            hpTextTweenScale.Begin();
            hpTextTweenScale.vector3Results = hpTextTweenScale.startingVector;
        }
        else
        {
            hpTweenColor.colorResults = hpTweenColor.endColor;
            hpSpriteTweenScale.vector3Results = hpSpriteTweenScale.endVector;
            hpTextTweenScale.vector3Results = hpTextTweenScale.endVector;
        }
    }

    void PlayShardTween(bool activeTween)
    {
        int tempCurrentShard = PlayerManager.Instance().PlayerStatus.CurrentMemoryShards;

        currentShard.text = tempCurrentShard.ToString();

        shardTweenColor.startingColor = Color.white;
        shardTweenColor.endColor = targetShardColor;

        if (activeTween)
        {
            shardTweenColor.Begin();
            shardTweenColor.colorResults = shardTweenColor.startingColor;

            shardSpriteTweenScale.Begin();
            shardSpriteTweenScale.vector3Results = shardSpriteTweenScale.startingVector;
            shardTextTweenScale.Begin();
            shardTextTweenScale.vector3Results = shardTextTweenScale.startingVector;
        }
        else
        {
            shardTweenColor.colorResults = shardTweenColor.endColor;
            shardSpriteTweenScale.vector3Results = shardSpriteTweenScale.endVector;
            shardTextTweenScale.vector3Results = shardTextTweenScale.endVector;
        }
    }

    void OnTriggerEnding()
    { 
        HPSprite.SetActive(false);
        currentHP.gameObject.SetActive(false);

        ShardSprite.SetActive(false);
        currentShard.gameObject.SetActive(false);
    } 

    private void Update()
    {
        currentHP.transform.localScale = hpTextTweenScale.vector3Results;
        HPSprite.transform.localScale = hpSpriteTweenScale.vector3Results;

        currentShard.transform.localScale = shardTextTweenScale.vector3Results;
        ShardSprite.transform.localScale = shardSpriteTweenScale.vector3Results;
    }
}
