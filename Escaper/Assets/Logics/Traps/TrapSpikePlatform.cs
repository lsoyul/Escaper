using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikePlatform : MonoBehaviour
{
    bool isStartActivate = false;
    bool showingSpikeState = false;

    float trapTimer = 0f;

    public SpriteRenderer platformSprite;
    public float activateTime = 2f;
    public TweenTransforms spikeTweener;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isStartActivate == false)
        {
            if (collision != null)
            {
                if (collision.gameObject.tag == "Player")
                {
                    if (showingSpikeState == false) isStartActivate = true;
                }
            }
        }
    }

    void Start()
    {
        trapTimer = 0;
    }

    void Update()
    {
        if (isStartActivate && !showingSpikeState)
        {
            trapTimer += Time.deltaTime;

            Color warnColor = Color.white;
            warnColor.g = 1 - (trapTimer / activateTime);
            warnColor.b = 1 - (trapTimer / activateTime);
            platformSprite.color = warnColor;

            if (trapTimer >= activateTime)
            {
                // Attack Spike!
                showingSpikeState = true;
                trapTimer = 0;

                spikeTweener.TweenCompleted += SpikeFinishEndCallback;
                spikeTweener.Begin();
                spikeTweener.defaultVector = spikeTweener.startingVector;

                platformSprite.color = Color.white;
            }
        }
    }

    void SpikeFinishEndCallback()
    {
        showingSpikeState = false;
        isStartActivate = false;

        spikeTweener.TweenCompleted -= SpikeFinishEndCallback;
    }
}
