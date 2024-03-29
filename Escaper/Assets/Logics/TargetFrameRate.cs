﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{
    public static bool isInit = false;
    public int targetFrame = 50;
    // Start is called before the first frame update
    private void Awake() {
        if (isInit == false)
        {
            DontDestroyOnLoad(this.gameObject);    
            isInit = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.targetFrameRate != targetFrame)
            Application.targetFrameRate = targetFrame;
    }
}
