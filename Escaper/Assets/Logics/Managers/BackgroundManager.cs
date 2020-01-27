using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Transform tracingPlayer;
    public Transform background;

    private Vector3 initPlayerPos;
    private Vector3 initBGPos;
    void Start()
    {
        initPlayerPos = new Vector3(tracingPlayer.position.x, tracingPlayer.position.y, tracingPlayer.position.z);
        initBGPos = new Vector3(background.position.x, background.position.y, background.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float playerDiff = tracingPlayer.position.y - initPlayerPos.y;
        Vector3 bgDiff = Vector2.up * playerDiff * 0.2f;

        background.transform.position = initBGPos - bgDiff;
    }
}
