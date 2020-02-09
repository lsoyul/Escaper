using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public CameraControlScript baseCamera;
    public Transform[] backgrounds;

    private Vector3 initCameraPos;
    private Vector3 initPlayerPos;
    private Vector3 initBGPos;
    void Start()
    {
        initCameraPos = new Vector3(baseCamera.transform.position.x, baseCamera.transform.position.y, baseCamera.transform.position.z);
        if (backgrounds != null)
        {
            if (backgrounds.Length > 0)
                initBGPos = new Vector3(backgrounds[0].position.x, backgrounds[0].position.y, backgrounds[0].position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float cameraXDiff = baseCamera.transform.position.x - initCameraPos.x;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (i != (backgrounds.Length-1))
            {
                Vector3 bgDiff = new Vector3();
                float xOffset = cameraXDiff * i * 0.15f;
                bgDiff.x = xOffset;
                backgrounds[i].transform.position = initBGPos - bgDiff;
            }
        }


    }
}
