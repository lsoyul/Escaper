using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrameRate : MonoBehaviour
{
    public int targetFrame = 60;
    // Start is called before the first frame update
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);    
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
