using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GlobalVariables;

public class CameraControl : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;

    public float lerpPower_Follow = 5f;
    public float lerpPower_Zoom = 7f;

    [Header("- Follow Camera -")]
    public float relationalZPosFromTarget;
    public float cameraHeight = 50f;
    public float cameraXRotation = 65.0f;

    [Header("- Entire Camera -")]
    public float cameraOrthographicSize = 190f;
    public float cameraXRotation_Entire = 90f;

    public CameraMode cameraMode = CameraMode.FOLLOW;

    private void Start()
    {
        mainCamera.transform.LookAt(target, Vector3.up);
    }

    public void ChangeView()
    {
        if (cameraMode == CameraMode.FOLLOW) cameraMode = CameraMode.ENTIRE_VIEW;
        else if (cameraMode == CameraMode.ENTIRE_VIEW) cameraMode = CameraMode.FOLLOW;
    }

    private void FixedUpdate()
    {
        if (cameraMode == CameraMode.FOLLOW)
        {
            //mainCamera.orthographic = false;

            Vector3 mainCameraTargetPos = new Vector3(target.position.x, cameraHeight, target.position.z - relationalZPosFromTarget);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCameraTargetPos, lerpPower_Follow * Time.fixedDeltaTime);

            float targetXRot = Mathf.LerpAngle(mainCamera.transform.rotation.eulerAngles.x, cameraXRotation, lerpPower_Zoom * Time.fixedDeltaTime);
            mainCamera.transform.eulerAngles = new Vector3(targetXRot, 0f, 0f);
        }
        else if (cameraMode == CameraMode.ENTIRE_VIEW)
        {
            //mainCamera.orthographic = true;

            Vector3 mainCameraTargetPos = new Vector3(0f, cameraOrthographicSize, 0f);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCameraTargetPos, lerpPower_Zoom * Time.fixedDeltaTime);

            //float targetOrthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraOrthographicSize, 5f * Time.fixedDeltaTime);
            //mainCamera.orthographicSize = targetOrthographicSize;

            float targetXRot = Mathf.LerpAngle(mainCamera.transform.rotation.eulerAngles.x, cameraXRotation_Entire, lerpPower_Zoom * Time.fixedDeltaTime);
            mainCamera.transform.eulerAngles = new Vector3(targetXRot, 0f, 0f);
        }
    }
}
