using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameStatics;

public class MemoryShard : MonoBehaviour
{
    private SHARD_TYPE shardType = SHARD_TYPE.SHARD1;

    bool isChasingTarget = false;
    bool isReachedTarget = false;

    private Transform chasingTargetTrans;

    private float curChaseSpeed = 0f;
    private float chaseIncSpeed = 2f;
    private float chaseDefaultSpeed = 1f;

    public float MAX_SPEED = 5f;

    public System.Action<SHARD_TYPE> onGetShard;

    public void SetShard(SHARD_TYPE shardType, Vector3 worldPos, Transform chasingTargetTrans)
    {
        this.shardType = shardType;
        this.transform.position = worldPos;
        this.chasingTargetTrans = chasingTargetTrans;
        curChaseSpeed = 0f;
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

                this.transform.position += (toVec * curChaseSpeed);

            }
            else if (isReachedTarget == false)
            {
                CheckChase();
            }
        }
    }


    void CheckChase()
    {
        if (Vector3.Distance(this.transform.position, this.chasingTargetTrans.position)
            < GameStatics.default_ShardsPullDistance + (GameStatics.default_IncShardsPullDistance * GameConfigs.SkillLevel_IncreaseShardsPullDistance))
        {
            StartChasing();
        }
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
                            if (onGetShard != null) onGetShard(shardType);
                            Destroy(this.gameObject);
                        }
                        break;
                }
            }
        }
    }
}
