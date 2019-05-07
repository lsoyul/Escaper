using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player targetPlayer;
    public MapGenerator mapGenerator;

    public Text posText;

    private void Start()
    {
        if (targetPlayer == null || mapGenerator == null) return;

        mapGenerator.onFinishGenerateMap += OnFinishGenerateMap;

    }


    void OnFinishGenerateMap()
    {
        Vector3 startPointVector =
            mapGenerator.CoordToPosition(mapGenerator.GetCurrentMap().startPoint.x, mapGenerator.GetCurrentMap().startPoint.y);

        targetPlayer.transform.position = startPointVector;
    }

    private void FixedUpdate()
    {
        posText.text = string.Format("Player pos : {0}.{1}", (int)targetPlayer.transform.position.x, (int)targetPlayer.transform.position.z);
    }
}
