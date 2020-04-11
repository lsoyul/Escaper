using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static GameStatics;

public class CameraControlScript : MonoBehaviour {

    //public UnityEngine.UI.Text text;
    public PlayerControllerScripts player;
    Transform cameraTransform;
    public FlickController flickController;
    Camera cam;

    [Header("- Camera Shake -")]
    
    public TweenTransforms tweener;
    bool isShaking = false;
    int cameraShakeRemainCount = 0;

    private Vector3 targetPos = new Vector3();

    private float cameraOrthogonalInitialSize = 160f;
    private bool camFixedXAxis = true;

    public float levelMinXAxis = -50f;
    public float levelMaxXAxis = 110f;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        PlayerManager.Instance().onDamaged += OnDamaged;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        PlayerManager.Instance().onDamaged -= OnDamaged;
    }

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

        targetPos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

                //float targetCamOrthogonalSize = cameraOrthogonalInitialSize;
                
                //if (flickController.GetIsHolding() == true)
                //    targetCamOrthogonalSize = cameraOrthogonalInitialSize + flickController.GetFlickedVector().magnitude * 0.1f;
                //cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetCamOrthogonalSize, 0.2f);
            
                //text.text = cam.orthographicSize.ToString();

                targetPos = player.transform.position;
                //targetPos.x = 0f;

                //if (flickController.GetIsHolding() == true && camFixedXAxis == false)
                //{
                //    targetPos.x = player.position.x + flickController.GetFlickedVector().x * -0.1f;
                //    targetPos.y = player.position.y + flickController.GetFlickedVector().y * -0.1f;
                //}

                if (targetPos.y < 0f) targetPos.y = 0f;
                if (camFixedXAxis) 
                {
                    if (targetPos.x < levelMinXAxis) targetPos.x = levelMinXAxis;
                    if (targetPos.x > levelMaxXAxis) targetPos.x = levelMaxXAxis;
                }
                
                cameraTransform.position = Vector2.Lerp(cameraTransform.position, targetPos, 0.2f);
                cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, -10f);
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
            case DAMAGED_TYPE.SPIKE:
            CameraShake(3);
            break;
        }
    }

    public void CameraShake(int count)
    {
        if (isShaking == false)
        {
            Vibration.Vibrate(2);
            cameraShakeRemainCount = count;
            isShaking = true;

            cameraShakeRemainCount--;
            tweener.Begin();
            tweener.transform.localPosition = tweener.startingVector;
            tweener.TweenCompleted += shakeCompleteEvent;
        }
    }

    void shakeCompleteEvent()
    {
        //Debug.Log("TweenComplete!");
        if (cameraShakeRemainCount > 0)
        {
            //Debug.Log("reset tween");
            cameraShakeRemainCount--;
            tweener.Reset();
            tweener.transform.localPosition = tweener.startingVector;

            if (cameraShakeRemainCount <= 0) 
            {
                //Debug.Log("shake finish");
                cameraShakeRemainCount = 0;
                isShaking = false;

                tweener.TweenCompleted -= shakeCompleteEvent;
            }
        }   
    }
}
