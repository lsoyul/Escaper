using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player targetPlayer;
    public MapGenerator mapGenerator;

    public Text posText;
    public Text indexText;

    bool initializedCurrentStage = false;

    private void Start()
    {
        if (targetPlayer == null || mapGenerator == null) return;

        mapGenerator.onFinishGenerateMap += OnFinishGenerateMap;
    }
    
    void OnFinishGenerateMap()
    {
        Vector3 startPointVector =
            mapGenerator.CoordToPosition(mapGenerator.GetCurrentMap().startPoint.x, mapGenerator.GetCurrentMap().startPoint.y);

        startPointVector.y = targetPlayer.transform.position.y;

        targetPlayer.transform.position = startPointVector;
        targetPlayer.InitializeVariables();

        initializedCurrentStage = true;
    }

    private void FixedUpdate()
    {
        MapGenerator.Coord playerCoord = mapGenerator.positionToCoord(targetPlayer.transform.position);

        posText.text = string.Format("Player pos : {0}.{1}", (int)targetPlayer.transform.position.x, (int)targetPlayer.transform.position.z);
        indexText.text = string.Format("Player index : {0},{1}", playerCoord.x, playerCoord.y);

        // Clear the current stage
        if (playerCoord.x == mapGenerator.GetCurrentMap().finishPoint.x 
            && playerCoord.y == mapGenerator.GetCurrentMap().finishPoint.y)
        {
            if (initializedCurrentStage == true)
            {
                initializedCurrentStage = false;
                mapGenerator.LoadNextStage();
            }
        }
    }
}
