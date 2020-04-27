using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using static GameStatics;

public class MemoryShard : MonoBehaviour
{
    private SHARD_TYPE shardType = SHARD_TYPE.SHARD1;

    bool isChasingTarget = false;
    bool isReachedTarget = false;

    private Transform chasingTargetTrans;

    private float curChaseSpeed = 0f;
    private float chaseIncSpeed = 4f;
    private float chaseDefaultSpeed = 1f;

    public float MAX_SPEED = 60f;

    public System.Action<MemoryShard> onGetShard;

    bool alreadyInitialize = false;

    #region ## Color Control ##

    public ParticleSystem[] psList;
    public Light2D backLight;

    public Color Color_HP;
    public Color Color_jumpCool;
    public Color Color_pullDist;

    public TweenColor tweenColor;

    #endregion

    public void SetShard(SHARD_TYPE shardType, Vector3 worldPos, Transform chasingTargetTrans, bool isChasingTarget = false)
    {
        this.shardType = shardType;
        this.transform.position = worldPos;
        this.chasingTargetTrans = chasingTargetTrans;
        curChaseSpeed = 0f;
        if (isChasingTarget)
        {
            StartChasing();
        }

        if (alreadyInitialize == false)
        {
            alreadyInitialize = true;
            TopMostControl.Instance().onFinishSceneFadeout += DestroyShard;
        }

        tweenColor.endColor = Color_pullDist;
    }

    public void SetSkillShardColor(SKILL_TYPE skillType)
    {
        tweenColor.startingColor = Color.white;
        switch (skillType)
        {
            case SKILL_TYPE.MAXHP:
                tweenColor.endColor = Color_HP;
                break;
            case SKILL_TYPE.AIRTIME_DURATION:
                tweenColor.endColor = Color_jumpCool;
                break;
            case SKILL_TYPE.SHARD_PULL_DIST:
                tweenColor.endColor = Color_pullDist;
                break;
            default:
                break;
        }

        tweenColor.Begin();
        tweenColor.colorResults = tweenColor.startingColor;
    }

    public void StartChasing()
    {
        isChasingTarget = true;
        curChaseSpeed = chaseDefaultSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (chasingTargetTrans != null)
        {
            if (isChasingTarget && !isReachedTarget)
            {
                Vector3 toVec = chasingTargetTrans.position - this.transform.position;
                toVec.Normalize();

                curChaseSpeed = curChaseSpeed + chaseIncSpeed * Time.deltaTime;

                if (curChaseSpeed > MAX_SPEED) curChaseSpeed = MAX_SPEED;

                if (Vector3.Distance(this.transform.position, chasingTargetTrans.position) < curChaseSpeed)
                {
                    this.transform.position = chasingTargetTrans.position;
                }
                else
                {
                    this.transform.position += (toVec * curChaseSpeed);
                }
            }
            else if (isReachedTarget == false)
            {
                CheckChase();
            }
        }

        // Color Control
        if (this.shardType == SHARD_TYPE.EFFECT)
        {
            foreach (ParticleSystem ps in psList)
            {
                var main = ps.main;
                main.startColor = tweenColor.colorResults;
            }

            backLight.color = tweenColor.colorResults;
        }
    }


    void CheckChase()
    {
        if (Vector3.Distance(this.transform.position, this.chasingTargetTrans.position)
            < GameStatics.GetShardPullDistance())
        {
            StartChasing();
        }
    }

    public SHARD_TYPE GetShardType()
    {
        return shardType;
    }

    public Color GetShardTargetColor()
    {
        return tweenColor.endColor;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider != null)
        {
            if (isChasingTarget == true && isReachedTarget == false)
            {
                switch (collider.tag)
                {
                    case "Player":
                        {
                            isReachedTarget = true;
                            if (onGetShard != null) onGetShard(this);
                            DestroyShard();
                        }
                        break;
                }
            }
        }
    }

    void DestroyShard()
    {
        if (this != null)
        {
            TopMostControl.Instance().onFinishSceneFadeout -= DestroyShard;
            Destroy(this.gameObject);
        }
    }
}
