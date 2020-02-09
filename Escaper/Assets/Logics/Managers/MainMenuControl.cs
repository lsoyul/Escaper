using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Experimental.Rendering.Universal;

using static GameStatics;

public class MainMenuControl : MonoBehaviour
{
    public FloatingJoystick floatingJoystick;

    public Light2D global2DLight;
    public Light2D light1;
    public GameObject mainTitle;
    
    [Header("- Animation -")]
    public TweenValue light1_intensityTween;
    public TweenValue light1_outherAngleTween;
    public TweenValue light1_intensityTweenFadeout;
    private bool light1_angleTweenStart = false;
    private bool light1_intensityFadeoutStart = false;

    public TweenValue globalLight2D_intensityBlink;
    private bool global2DLight_blink = false;

    public TweenTransforms mainTitle_transformTween;
    private bool mainTitle_animatingStart = false;

    public TweenTransforms Btn_Newgame;
    public TweenTransforms Btn_Continue;

    bool isEndAnimating = false;

    [Header("- Camera -")]
    public Camera mainCam;
    private float cameraOrthogonalInitialSize = 160f;
    

    // Start is called before the first frame update
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // ---------- Camera --------------
        // 1280 x 720 : cameraOrthogonalInitialSize
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 720f / 1280f;
        if (screenRatio >= targetRatio){
            mainCam.orthographicSize = cameraOrthogonalInitialSize;
        }
        else{
            float differenceInSize = targetRatio / screenRatio;
            mainCam.orthographicSize = cameraOrthogonalInitialSize * differenceInSize;
        }
    }
    void Start()
    {
        Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;
        Btn_Continue.transform.localPosition = Btn_Continue.startingVector;
        mainTitle.transform.localPosition = mainTitle_transformTween.startingVector;

        floatingJoystick.onPointerDown += OnPointerDown;

        // Animating
        light1_intensityTween.TweenCompleted += () => {
            //if (!isEndAnimating)
            //{   
                light1_angleTweenStart = true;
                light1_outherAngleTween.Begin();
                light1_outherAngleTween.value = light1_outherAngleTween.startValue;
            //}
        };

        light1_outherAngleTween.TweenCompleted += () => {
            light1_intensityFadeoutStart = true;
            if (!isEndAnimating) {
                light1_angleTweenStart = false;
                global2DLight_blink = true;

                light1_intensityTweenFadeout.Begin();
                light1_intensityTweenFadeout.value = light1_intensityTweenFadeout.startValue;

                globalLight2D_intensityBlink.Begin();
                globalLight2D_intensityBlink.value = globalLight2D_intensityBlink.startValue;
            }
        };

        light1_intensityTweenFadeout.TweenCompleted += () => {
            if (!isEndAnimating && !mainTitle_animatingStart) {
                mainTitle_animatingStart = true;

                mainTitle_transformTween.Begin();
                mainTitle.transform.localPosition = mainTitle_transformTween.startingVector;

                Btn_Newgame.Begin();
                Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;

                Btn_Continue.Begin();
                Btn_Continue.transform.localPosition = Btn_Continue.startingVector;
            }
        };

        mainTitle_transformTween.TweenCompleted += () => {
            isEndAnimating = true;
        };

        
    }

    // Update is called once per frame
    void Update()
    {
#region Animating

        if (light1_intensityFadeoutStart == false)
        {
            light1.intensity = light1_intensityTween.value;
            if (light1.intensity < light1_intensityTween.startValue) light1.intensity = light1_intensityTween.startValue;
        }
        else
        {
            light1.intensity = light1_intensityTweenFadeout.value;
        }

        if (light1_angleTweenStart) 
        {
            light1.pointLightInnerAngle = 1f;
            light1.pointLightOuterAngle = light1_outherAngleTween.value;
            //if (light1.pointLightOuterAngle < light1_outherAngleTween.startValue) light1.pointLightOuterAngle = light1_outherAngleTween.startValue;
        }

        if (global2DLight_blink)
        {
            global2DLight.intensity = globalLight2D_intensityBlink.value;
        }

        if (mainTitle_animatingStart)
        {
            mainTitle.transform.localPosition = mainTitle_transformTween.transform.position;
        }

#endregion

    }

    void OnPointerDown()
    {
        if (isEndAnimating == false)
        {
            isEndAnimating = true;
            light1_intensityTween.Stop();
            light1_intensityTweenFadeout.Stop();
            light1_outherAngleTween.Stop();
            globalLight2D_intensityBlink.Stop();

            light1.intensity = light1_intensityTweenFadeout.endValue;
            light1.pointLightInnerAngle = 1f;
            light1.pointLightOuterAngle = light1_outherAngleTween.endValue;

            global2DLight.intensity = globalLight2D_intensityBlink.endValue;

            if (mainTitle_animatingStart == false)
            {
                mainTitle_transformTween.Begin();
                mainTitle.transform.localPosition = mainTitle_transformTween.startingVector;
                
                Btn_Newgame.Begin();
                Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;

                Btn_Continue.Begin();
                Btn_Continue.transform.localPosition = Btn_Continue.startingVector;

                mainTitle_animatingStart = true;
            }
            else
            {
                // expose all finish menus
            }
        }
    }

    public void OnClickNewGame()
    {
        TopMostControl.Instance().StartChangeScene(SCENE_INDEX.GAMING, true);
    }
}
