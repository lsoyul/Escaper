using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class GameManager : MonoBehaviour
{
    private GameStatics.GAME_STATUS currentGameStatus;
    public GameStatics.GAME_STATUS GameStatus
    {
        get { return currentGameStatus; }
    }

    public Action<GameStatics.GAME_STATUS, GameStatics.GAME_STATUS> onChangeGameStatus;
    

    private static GameManager instance;
    public GameManager Instance
    {
        get {
            if (instance == null) instance = GetComponent<GameManager>();
            
            return instance;
        }
    }

    private void Awake() {
        
        if (instance == null) 
        {
            instance = GetComponent<GameManager>();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        currentGameStatus = GameStatics.GAME_STATUS.NONE;
        ChangeGameStatus(GameStatics.GAME_STATUS.SPLASH);
    }
    
    void Start()
    {
        Input.multiTouchEnabled = false;
        //Screen.SetResolution(720, 1280, true, 60);
    }

    public void ChangeGameStatus(GameStatics.GAME_STATUS nextStatus)
    {
        GameStatics.GAME_STATUS beforeGameStatus = currentGameStatus;
        currentGameStatus = nextStatus;

        if (onChangeGameStatus != null) onChangeGameStatus(beforeGameStatus, currentGameStatus);
    }
}
