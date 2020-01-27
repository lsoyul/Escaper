using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public GameManager Instance
    {
        get {
            if (instance == null) instance = GetComponent<GameManager>();
            
            return instance;
        }
    }

    private void Awake() {
        DontDestroyOnLoad(this);
        if (instance == null) instance = new GameManager();
    }
    
    void Start()
    {
        Input.multiTouchEnabled = false;
        //Screen.SetResolution(720, 1280, true, 60);
    }
}
