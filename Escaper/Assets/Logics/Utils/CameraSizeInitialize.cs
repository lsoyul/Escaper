using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeInitialize : MonoBehaviour
{
    Camera cam;
    private float targetOrthogonalSize;
    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();

        targetOrthogonalSize = cam.orthographicSize;

        // 1280 x 720 : cameraOrthogonalInitialSize
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 720f / 1280f;
        if (screenRatio >= targetRatio){
            cam.orthographicSize = targetOrthogonalSize;
        }
        else{
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = targetOrthogonalSize * differenceInSize;
        }
    }
}
