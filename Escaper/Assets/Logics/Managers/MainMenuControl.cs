using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Experimental.Rendering.Universal;
using DigitalRuby.SoundManagerNamespace;

using static GameStatics;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    public Light2D global2DLight;
    public Light2D light1;
    public GameObject mainTitle;

    public Camera cam;
    
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

    public TweenValue global2DLight_Ending1_intensityTween;
    public Light2D global2DLight_Ending1;

    [Header("- Buttons -")]
    //public GameObject Btn_GoogleLogin;
    //public GameObject Btn_AnonymousPlay;
    //public GameObject Btn_Logout;

    public TweenTransforms Btn_Newgame;
    public TweenTransforms Btn_ChangeMode;
    public Text Btn_NewGameText;
    public Button NewGameButton;

    bool isEndAnimating = false;

    [Header("- After Ending Control -")]
    public GameObject Show_NormalObject;
    public GameObject Show_Ending1Object;

    public Text ChangeModeText;

    public TweenTransforms ChangeModeCameraTween;

    [Header("- DebugTest -")]
    public UnityEngine.UI.Text debugText;


    // ## Input ##
    private bool touchOn = false;
    private Touch tempTouchs;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
    void Start()
    {
        // -- Camera Init --

        // 9 x 16
        // 1600 x 900 : cameraOrthogonalInitialSize
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 9f / 16f;
        if (screenRatio >= targetRatio)
        {
            cam.orthographicSize = 160f;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = 160f * differenceInSize;
        }
        // -- Camera Init --

        if (PlayerManager.Instance().PlayMode == PLAY_MODE.TRUE)
        {
            Show_NormalObject.SetActive(false);
            Show_Ending1Object.SetActive(true);

            mainTitle_animatingStart = true;

            mainTitle_transformTween.Begin();
            mainTitle.transform.localPosition = mainTitle_transformTween.startingVector;

            Btn_Newgame.Begin();
            Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;

            if (GameConfigs.GetNormalEnding() == true)
            {
                Btn_ChangeMode.Begin();
                Btn_ChangeMode.transform.localPosition = Btn_ChangeMode.startingVector;
            }

            global2DLight.intensity = globalLight2D_intensityBlink.endValue;
        }
        else
        {
            Show_NormalObject.SetActive(true);
            Show_Ending1Object.SetActive(false);
        }


        if (GameConfigs.GetNormalEnding() == true)
        {
            SetChangeModeText(PlayerManager.Instance().PlayMode);
        }


        Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;
        Btn_ChangeMode.transform.localPosition = Btn_ChangeMode.startingVector;
        mainTitle.transform.localPosition = mainTitle_transformTween.startingVector;

        //TopMostControl.Instance().GetController().onPointerDown += OnPointerDown;

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
            if (!isEndAnimating && !mainTitle_animatingStart)
            {
                mainTitle_animatingStart = true;

                mainTitle_transformTween.Begin();
                mainTitle.transform.localPosition = mainTitle_transformTween.startingVector;

                Btn_Newgame.Begin();
                Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;

                if (GameConfigs.GetNormalEnding() == true)
                {
                    Btn_ChangeMode.Begin();
                    Btn_ChangeMode.transform.localPosition = Btn_ChangeMode.startingVector;
                }
            }
        };

        mainTitle_transformTween.TweenCompleted += () => {
            isEndAnimating = true;
            TopMostControl.Instance().SettingShowButton.SetActive(true);
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

        if (GameConfigs.GetNormalEnding() == true)
        {
            global2DLight_Ending1.intensity = global2DLight_Ending1_intensityTween.value;
        }

        #endregion

        #region # Input Control #

#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
        {
            if (touchOn == false)
            {
                touchOn = true;
                OnPointerDown();
            }
        }

#else
        if (Input.touchCount > 0)
        {    
            for (int i = 0; i < Input.touchCount; i++)
            {
                tempTouchs = Input.GetTouch(i);
                if (tempTouchs.phase == TouchPhase.Began)
                {
                    if (touchOn == false) { 
                        touchOn = true;
                        OnPointerDown();
                    }
                    break;   
                }
            }
        }
#endif

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

                if (GameConfigs.GetNormalEnding() == true)
                {
                    Btn_ChangeMode.Begin();
                    Btn_ChangeMode.transform.localPosition = Btn_ChangeMode.startingVector;
                }

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
        if (TopMostControl.Instance().GetIsSceneChanging() == false)
        {
            TopMostControl.Instance().StartChangeScene(SCENE_INDEX.CUTSCENE, true);
            SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select].clip);

            if (PlayerManager.Instance().PlayMode == PLAY_MODE.NORMAL)
                Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_START_GAME_NORMAL);
            else if (PlayerManager.Instance().PlayMode == PLAY_MODE.TRUE)
                Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_START_GAME_TRUEHERO);
        }
    }

    public void OnClickChangePlayMode()
    {
        if (PlayerManager.Instance().PlayMode == PLAY_MODE.NORMAL)
        {
            PlayerManager.Instance().PlayMode = PLAY_MODE.TRUE;
            GameConfigs.SetLastPlayMode(PLAY_MODE.TRUE);
            Show_NormalObject.SetActive(false);
            Show_Ending1Object.SetActive(true);

            SoundManager.PlayLoopingMusic(SoundContainer.Instance().BackGroundMusicsDic["BGM_Ending"], 1.0f, 1.0f, true);
        }
        else
        {
            PlayerManager.Instance().PlayMode = PLAY_MODE.NORMAL;
            GameConfigs.SetLastPlayMode(PLAY_MODE.NORMAL);
            Show_NormalObject.SetActive(true);
            Show_Ending1Object.SetActive(false);

            SoundManager.PlayLoopingMusic(SoundContainer.Instance().BackGroundMusicsDic["Opening"], 1.0f, 1.0f, true);
        }

        SetChangeModeText(PlayerManager.Instance().PlayMode);
        ChangeModeCameraTween.Begin();
        ChangeModeCameraTween.vector3Results = ChangeModeCameraTween.startingVector;

        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1].clip);
    }

    void SetChangeModeText(PLAY_MODE playMode)
    {
        switch (playMode)
        {
            case PLAY_MODE.NORMAL:
                ChangeModeText.text = "NORMAL";
                ChangeModeText.color = Color.green;
                Btn_NewGameText.color = Color.white;
                NewGameButton.interactable = true;
                break;
            case PLAY_MODE.TRUE:
                ChangeModeText.text = "TRUE HERO";
                ChangeModeText.color = Color.cyan;
                Btn_NewGameText.color = Color.gray;
                NewGameButton.interactable = false;
                break;
            default:
                ChangeModeText.text = "NORMAL";
                ChangeModeText.color = Color.green;
                break;
        }
    }

}
