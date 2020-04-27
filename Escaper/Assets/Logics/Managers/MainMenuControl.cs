using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Experimental.Rendering.Universal;
using DigitalRuby.SoundManagerNamespace;

using static GameStatics;

public class MainMenuControl : MonoBehaviour
{
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

    [Header("- Buttons -")]
    //public GameObject Btn_GoogleLogin;
    //public GameObject Btn_AnonymousPlay;
    //public GameObject Btn_Logout;

    public TweenTransforms Btn_Newgame;
    public TweenTransforms Btn_Continue;

    bool isEndAnimating = false;

    [Header("- DebugTest -")]
    public UnityEngine.UI.Text debugText;


    // ## Input ##
    private bool touchOn = false;
    private Touch tempTouchs;

    private void OnEnable()
    {
        //GameManager.Instance().onLoginFinish += OnGoogleLoginFinish;
        //GameManager.Instance().onSignout += OnSignOut;
    }

    private void OnDisable()
    {
        //GameManager.Instance().onLoginFinish -= OnGoogleLoginFinish;
        //GameManager.Instance().onSignout -= OnSignOut;
    }
    void Start()
    {
        //Btn_Logout.SetActive(false);

        Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;
        Btn_Continue.transform.localPosition = Btn_Continue.startingVector;
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
        if (TopMostControl.Instance().GetIsSceneChanging() == false)
        {
            TopMostControl.Instance().StartChangeScene(SCENE_INDEX.CUTSCENE, true);
            SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select].clip);
        }
    }



#if UNITY_EDITOR
    //public void ClickGoogleLogin()
    //{
    //    OnPointerDown();
    //    Btn_GoogleLogin.SetActive(false);
    //    Btn_AnonymousPlay.SetActive(false);
    //    OnGoogleLoginFinish(LOGIN_TYPE.ANONYMOUS);
    //}

    //public void ClickGoogleLogout()
    //{
    //    //Btn_Logout.SetActive(false);
    //
    //    Btn_Newgame.Stop();
    //    Btn_Continue.Stop();
    //    Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;
    //    Btn_Continue.transform.localPosition = Btn_Continue.startingVector;
    //
    //    OnSignOut();
    //}

    //public void ClickWithoutLogin()
    //{
    //    OnPointerDown();
    //    Btn_GoogleLogin.SetActive(false);
    //    Btn_AnonymousPlay.SetActive(false);
    //    OnGoogleLoginFinish(LOGIN_TYPE.ANONYMOUS);
    //}
#else
    //public void ClickGoogleLogin()
    //{
    //    OnPointerDown();
    //    Btn_GoogleLogin.SetActive(false);
    //    Btn_AnonymousPlay.SetActive(false);
    //    GameManager.Instance().OnClickGoogleLogin();
    //}
    //
    //public void ClickGoogleLogout()
    //{
    //    Btn_Logout.SetActive(false);
    //    Btn_Newgame.Stop();
    //    Btn_Continue.Stop();
    //    Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;
    //    Btn_Continue.transform.localPosition = Btn_Continue.startingVector;
    //    GameManager.Instance().OnClickGoogleLogout();
    //}
    //
    //public void ClickWithoutLogin()
    //{
    //    OnPointerDown();
    //    Btn_GoogleLogin.SetActive(false);
    //    Btn_AnonymousPlay.SetActive(false);
    //    GameManager.Instance().SetAnonymousPlay();
    //}
#endif

    //void OnGoogleLoginFinish(GameStatics.LOGIN_TYPE loginType)
    //{
    //    switch (loginType)
    //    {
    //        case LOGIN_TYPE.FAIL:
    //            debugText.text = "Offline";
    //            Btn_GoogleLogin.SetActive(true);
    //            Btn_AnonymousPlay.SetActive(true);
    //            break;
    //        case LOGIN_TYPE.ANONYMOUS:
    //            debugText.text = "Guest";
    //            Btn_Logout.SetActive(true);
    //            break;
    //        case LOGIN_TYPE.GOOGLE:
    //            debugText.text = Social.localUser.userName;
    //            Btn_Logout.SetActive(true);
    //            break;
    //        default:
    //            break;
    //    }
    //
    //
    //    Btn_Newgame.Begin();
    //    Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;
    //
    //    Btn_Continue.Begin();
    //    Btn_Continue.transform.localPosition = Btn_Continue.startingVector;
    //}

    //void OnSignOut()
    //{
    //    Btn_GoogleLogin.SetActive(true);
    //    Btn_AnonymousPlay.SetActive(true);
    //    Btn_Logout.SetActive(false);
    //
    //    Btn_Newgame.transform.localPosition = Btn_Newgame.startingVector;
    //    Btn_Continue.transform.localPosition = Btn_Continue.startingVector;
    //
    //    debugText.text = "Offline";
    //}
}
