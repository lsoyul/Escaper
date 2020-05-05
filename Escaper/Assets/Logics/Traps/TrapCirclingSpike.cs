using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCirclingSpike : MonoBehaviour
{
    public TweenTransforms spikeBodyTweener;
    public float circlingSpeedRatio = 3600;

    // Start is called before the first frame update
    void Start()
    {
        spikeBodyTweener.startingVector = Vector3.zero;
        spikeBodyTweener.endVector = new Vector3(0, 0, circlingSpeedRatio);
        spikeBodyTweener.Begin();
    }

}
