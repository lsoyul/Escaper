using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour {

    //public UnityEngine.UI.Text text;
    public Transform player;
    Transform cameraTransform;
    public FlickController flickController;
    Camera cam;

    private Vector3 targetPos = new Vector3();

    private float cameraOrthogonalInitialSize = 160f;
    private bool camFixedXAxis = true;
    public UnityEngine.UI.Text camModeText;

	// Use this for initialization
        void Start () {
        cameraTransform = this.GetComponent<Transform>();
        cam = this.GetComponent<Camera>();

        // 1280 x 720 : cameraOrthogonalInitialSize
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 720f / 1280f;
        if (screenRatio >= targetRatio){
            cam.orthographicSize = cameraOrthogonalInitialSize;
        }
        else{
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = cameraOrthogonalInitialSize * differenceInSize;
        }

        camModeText.text = string.Format("Change CamMode\nFix-XAxis : {0}", camFixedXAxis);
        targetPos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

                //float targetCamOrthogonalSize = cameraOrthogonalInitialSize;
                
                //if (flickController.GetIsHolding() == true)
                //    targetCamOrthogonalSize = cameraOrthogonalInitialSize + flickController.GetFlickedVector().magnitude * 0.1f;
                //cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetCamOrthogonalSize, 0.2f);
            
                //text.text = cam.orthographicSize.ToString();

                targetPos = player.position;
                //targetPos.x = 0f;

                if (flickController.GetIsHolding() == true && camFixedXAxis == false)
                {
                    targetPos.x = player.position.x + flickController.GetFlickedVector().x * -0.1f;
                    targetPos.y = player.position.y + flickController.GetFlickedVector().y * -0.1f;
                }

                if (targetPos.y < 0f) targetPos.y = 0f;
                if (camFixedXAxis) targetPos.x = 0f;

                cameraTransform.position = Vector2.Lerp(cameraTransform.position, targetPos, 0.2f);
                cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, -10f);
	}

    public void ChangeCameraMode()
    {
        camFixedXAxis = !camFixedXAxis;

        camModeText.text = string.Format("Change CamMode\nFix-XAxis : {0}", camFixedXAxis);
    }
}
