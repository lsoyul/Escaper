using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

using static GameStatics;

public class CameraControlScript : MonoBehaviour {

    //public UnityEngine.UI.Text text;
    private PlayerControllerScripts player;
    Transform cameraTransform;
    public FlickController flickController;
    Camera cam;

    [Header("- Global Light -")]
    public Light2D globalLight;

    [Header("- Camera Shake -")]
    
    public TweenTransforms tweener;

    public bool IsShakingCamera;

    private Vector3 targetPos = new Vector3();

    private float cameraOrthogonalInitialSize = 160f;
    private bool camFixedXAxis = true;

    public float levelMinXAxis = -50f;
    public float levelMaxXAxis = 110f;


    private float UpgradeUIYOffset = 110f;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        PlayerManager.Instance().onDamaged += OnDamaged;
        TopMostControl.Instance().onCameraShake += CameraShake_Rot;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        if (PlayerManager.HasInstance())
        {
            PlayerManager.Instance().onDamaged -= OnDamaged;
        }

        if (TopMostControl.HasInstance())
        {
            TopMostControl.Instance().onCameraShake -= CameraShake_Rot;
        }
    }

    private void Awake()
    {
        player = PlayerManager.Instance().GetPlayerControl();

        PlayerManager.Instance().SetCameraController(this);
    }

    // Use this for initialization
    void Start () 
    {
        cameraTransform = this.GetComponent<Transform>();
        cam = this.GetComponent<Camera>();

        // 9 x 16
        // 1600 x 900 : cameraOrthogonalInitialSize
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 9f / 16f;
        if (screenRatio >= targetRatio){
            cam.orthographicSize = cameraOrthogonalInitialSize;
        }
        else{
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = cameraOrthogonalInitialSize * differenceInSize;
        }

        targetPos = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        targetPos = player.transform.position;

        if (targetPos.y < GameStatics.GetCameraMinimumYAxis(StageLoader.CurrentStage)) targetPos.y = GetCameraMinimumYAxis(StageLoader.CurrentStage);
        if (camFixedXAxis)
        {
            if (targetPos.x < levelMinXAxis) targetPos.x = levelMinXAxis;
            if (targetPos.x > levelMaxXAxis) targetPos.x = levelMaxXAxis;
        }

        // Stage3 Last Camera Move
        if (StageLoader.CurrentStage == 3)
        {
            if (player.transform.position.y > 1715 &&
                player.transform.position.x > -3)
            {
                targetPos.x = 82;
                targetPos.y = 1718;
            }
        }

        // Check Upgrade Status
        if (TopMostControl.Instance().GetGameUIStatus() == TOPUI_STATUS.GAMEOVER)
        {
            targetPos.y = player.transform.position.y + UpgradeUIYOffset;
        }

        if (IsShakingCamera == false)
        {
            cameraTransform.position = Vector2.Lerp(cameraTransform.position, targetPos, 0.2f);
            cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, -10f);
        }
    }

    public void ChangeCameraMode()
    {
        camFixedXAxis = !camFixedXAxis;
    }

    public Vector3 GetTargetPos(){
        return targetPos;
    }

    private void OnDamaged(DAMAGED_TYPE damagedType)
    {
        switch (damagedType)
        {
            case DAMAGED_TYPE.FALLING_GROUND:
                Vibration.Vibrate(5);
                CameraShake_Rot(5);
                break;
            default:
                Vibration.Vibrate(3);
                CameraShake_Rot(2);
                break;
        }
    }

    public void SetGlobalLightIntensity(float intensity)
    {
        globalLight.intensity = intensity;
    }

    public void CameraShake_Rot(int count)
    {
        if (IsShakingCamera == false)
            StartCoroutine(ShakeCamera(count));
    }

    IEnumerator ShakeCamera(int count)
    {
        IsShakingCamera = true;
        int currentCount = 0;

        while (currentCount < count)
        {
            currentCount++;

            tweener.startingVector = new Vector3(0, 0, -1);
            tweener.endVector = new Vector3(0, 0, 1);
            tweener.Begin();
            tweener.vector3Results = Vector3.zero;
            yield return new WaitForSeconds(tweener.duration);
        }

        // End Shake

        tweener.endVector = Vector3.zero;
        tweener.Stop();
        IsShakingCamera = false;
        //tweener.vector3Results = Vector3.zero;
        //tweener.transform.rotation = Vector3.zero;
    }

    //public void CameraShake(int count)
    //{
    //        //Vibration.Vibrate(2);
    //    cameraShakeRemainCount = count;
    //
    //    cameraShakeRemainCount--;
    //    tweener.Begin();
    //    tweener.transform.localPosition = tweener.startingVector;
    //    tweener.TweenCompleted += shakeCompleteEvent;
    //}
    //
    //void shakeCompleteEvent()
    //{
    //    //Debug.Log("TweenComplete!");
    //    if (cameraShakeRemainCount > 0)
    //    {
    //        //Debug.Log("reset tween");
    //        cameraShakeRemainCount--;
    //        tweener.Reset();
    //        tweener.transform.localPosition = tweener.startingVector;
    //
    //        if (cameraShakeRemainCount <= 0) 
    //        {
    //            //Debug.Log("shake finish");
    //            cameraShakeRemainCount = 0;
    //
    //            tweener.TweenCompleted -= shakeCompleteEvent;
    //        }
    //    }   
    //}
}
