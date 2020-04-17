using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBase : MonoBehaviour
{
    public Transform PlayerInitPos;
    public Transform shardBaseRoot;

    public List<Transform> ShardPosList()
    {
        List<Transform> posList = new List<Transform>();

        foreach(Transform tran in shardBaseRoot)
        {
            posList.Add(tran);
        }

        return posList;
    }

    public Transform GetPlayerInitPos()
    {
        return PlayerInitPos;
    }
}
