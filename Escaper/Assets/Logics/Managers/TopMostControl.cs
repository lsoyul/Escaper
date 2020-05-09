using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using DigitalRuby.SoundManagerNamespace;

using static GameStatics;

public class TopMostControl : MonoBehaviour
{
    public GameObject topMostBlackPanel;
    public TweenAlpha blackPanelAlpha;
    public Canvas topCanvas;
    public GameObject gameControlObj;

    public Color joystick_backColor;
    public Color joystick_centerColor;

    private SCENE_INDEX currentTargetScene;
    private bool isChangingState = false;

    public System.Action<int> onCameraShake;
    public System.Action onFinishSceneFadeout;


    private static GameObject container;
    private static TopMostControl instance;
    public static TopMostControl Instance()
    {
        //if (instance == null)
        //{
        //    container = new GameObject();
        //    container.name = "TopMostControl";
        //    instance = container.AddComponent(typeof(TopMostControl)) as TopMostControl;
        //}

        return instance;
    }

    public static bool HasInstance()
    {
        return instance;
    }

    private void Awake() {
        
        if (instance == null)
        {
            container = this.gameObject;
            container.name = "TopMostControl";
            instance = GetComponent<TopMostControl>();
            DontDestroyOnLoad(this);

        }
        gameControlObj.SetActive(false);
        SoundManager.StopSoundsOnLevelLoad = false;

    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    #region #### Flick Controller ####

    public FlickController flickController;

    public FlickController GetController()
    {
        return flickController;
    }

    #endregion


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isChangingState = false;
        blackPanelAlpha.startValue = 1f;
        blackPanelAlpha.endValue = 0f;
        blackPanelAlpha.TweenCompleted += SceneFadeinFinishEvent;
        blackPanelAlpha.Begin();
        blackPanelAlpha.value = blackPanelAlpha.startValue;

        if (scene.buildIndex == (int)SCENE_INDEX.GAMESTAGE)
        {
            StageLoader.Instance().SetStage(StageLoader.NextStage);
            gameControlObj.SetActive(true);
            topCanvas.sortingOrder = 10;

            ReturnButton.SetActive(true);
            SettingShowButton.SetActive(true);

            if (StageLoader.CurrentStage == 1)
            {
                PlayerManager.Instance().playerController.isFainting = true;
            }

            // #### TEST
            testscript.gameObject.SetActive(true);
        }
        else
        {
            StageLoader.Instance().DisableStage();
            gameControlObj.SetActive(false);
            topCanvas.sortingOrder = 0;

            ReturnButton.SetActive(false);
            SettingShowButton.SetActive(false);

            testscript.gameObject.SetActive(false);
        }

        StartBGM(scene);
        ChangeEnvironment(scene);
    }

    public void StartBGM(Scene targetScene)
    {
        SoundManager.StopAllLoopingSounds();

        switch (targetScene.buildIndex)
        {
            case (int)SCENE_INDEX.MAINMENU:
                SoundManager.PlayLoopingMusic(SoundContainer.Instance().BackGroundMusicsDic["Opening"], 1.0f, 1.0f, true);
                break;
            case (int)SCENE_INDEX.GAMESTAGE:
                if (StageLoader.CurrentStage == 1)
                {
                    SoundManager.PlayLoopingMusic(SoundContainer.Instance().BackGroundMusicsDic["BGM_Stage1"], 1.0f, 1.0f, true);
                }
                else if (StageLoader.CurrentStage == 2)
                {
                    SoundManager.PlayLoopingMusic(SoundContainer.Instance().BackGroundMusicsDic["BGM_Stage2"], 1.0f, 1.0f, true);
                }
                break;

            default:
                break;
        }
    }

    public void ChangeEnvironment(Scene targetScene)
    {
        switch (targetScene.buildIndex)
        {
            case (int)SCENE_INDEX.MAINMENU:
                break;
            case (int)SCENE_INDEX.GAMESTAGE:
                if (StageLoader.CurrentStage == 1)
                {
                    PlayerManager.Instance().CameraController().SetGlobalLightIntensity(0.8f);
                }
                else if (StageLoader.CurrentStage == 2)
                {
                    PlayerManager.Instance().CameraController().SetGlobalLightIntensity(1.0f);
                }
                break;

            default:
                break;
        }
    }

    public void StartChangeScene(SCENE_INDEX targetSceneIndex, bool smoothChange, int nextStageNum = 1)
    {
        topCanvas.sortingOrder = 10;
        isChangingState = true;
        blackPanelAlpha.startValue = 0f;
        blackPanelAlpha.endValue = 1f;
        currentTargetScene = targetSceneIndex;
        blackPanelAlpha.TweenCompleted += SceneFadeoutFinishEvent;
        blackPanelAlpha.Begin();
        blackPanelAlpha.value = blackPanelAlpha.startValue;

        StageLoader.NextStage = nextStageNum;
    }

    void SceneFadeoutFinishEvent()
    {
        if (isChangingState)
        {
            SceneManager.LoadSceneAsync((int)currentTargetScene, LoadSceneMode.Single);
            blackPanelAlpha.TweenCompleted -= SceneFadeoutFinishEvent;

            if (onFinishSceneFadeout != null) onFinishSceneFadeout();
        }
    }

    void SceneFadeinFinishEvent()
    {
        blackPanelAlpha.TweenCompleted -= SceneFadeinFinishEvent;

        if (SceneManager.GetActiveScene().buildIndex == (int)SCENE_INDEX.GAMESTAGE)
        {
            // Show SubTitle
            ShowSubTitle(StageLoader.CurrentStage);
        }
    }

    public bool GetIsSceneChanging()
    {
        return isChangingState;
    }

    public TOPUI_STATUS currentGameUIStatus = TOPUI_STATUS.NORMAL;

    public TOPUI_STATUS GetGameUIStatus()
    {
        return currentGameUIStatus;
    }

    #region ### SecondWind UI ###

    [Header("- SecondWind UI -")]
    bool isShowingSecondWind = false;
    public TweenTransforms secondWindTweenTrans;
    public UnityEngine.UI.Text shardsAmountsToRevive;

    private int currentRequiredShardsAmounts = int.MaxValue;

    public GameObject NoMoreRevivePanel;

    private Vector3 outXPos_gameOverUI = new Vector3(900, -256, 0);
    private Vector3 inXPos_gameOverUI = new Vector3(293, -256, 0);

    public System.Action<MENU_GAMEOVER> onClickGameOverMenu;

    public int GetRequiredShardsForRevive()
    {
        return currentRequiredShardsAmounts;
    }

    public void GameOver(bool isShow)
    {
        ShowUpgradeUI(isShow);

        if (PlayerManager.Instance().PlayerStatus.RemainReviveCount > 0)
        {
            if (isShow)
            {
                if ((PlayerManager.Instance().PlayerStatus.CurrentMemoryShards / 2) > 0)
                    NoMoreRevivePanel.SetActive(false);
                else
                    NoMoreRevivePanel.SetActive(true);
                
                ShowSecondWind(true);
            }
            else ShowSecondWind(false);
        }
        else
        {
            NoMoreRevivePanel.SetActive(true);
            ShowSecondWind(false);
        }

        if (isShow)
            currentGameUIStatus = TOPUI_STATUS.GAMEOVER;
        else
            currentGameUIStatus = TOPUI_STATUS.NORMAL;
    }

    public void ShowSecondWind(bool isShow)
    {
        if (isShow)
        {
            currentRequiredShardsAmounts = PlayerManager.Instance().PlayerStatus.CurrentMemoryShards / 2;
            shardsAmountsToRevive.text = "-" + currentRequiredShardsAmounts.ToString();
            secondWindTweenTrans.startingVector = outXPos_gameOverUI;
            secondWindTweenTrans.endVector = inXPos_gameOverUI;
        }
        else
        {
            currentRequiredShardsAmounts = int.MaxValue;
            secondWindTweenTrans.startingVector = inXPos_gameOverUI;
            secondWindTweenTrans.endVector = outXPos_gameOverUI;
        }

        if ((isShow == false && isShowingSecondWind == false)
            || (isShow == true && isShowingSecondWind == true))
        {
            secondWindTweenTrans.defaultVector = secondWindTweenTrans.endVector;
        }
        else
        {
            secondWindTweenTrans.Begin();
            secondWindTweenTrans.defaultVector = secondWindTweenTrans.startingVector;
        }

        isShowingSecondWind = isShow;
    }

    public void OnClick_GO_Back()
    {
        OnClickSettingHide();
        if (SceneManager.GetActiveScene().buildIndex == (int)SCENE_INDEX.GAMESTAGE)
        {
            if (isShowingUpgradeUI) GameOver(false);
            StartChangeScene(SCENE_INDEX.MAINMENU, true);
            if (onClickGameOverMenu != null) onClickGameOverMenu(MENU_GAMEOVER.MAINMENU);
        }
        else if (SceneManager.GetActiveScene().buildIndex == (int)SCENE_INDEX.MAINMENU)
        {
            // Application Quit
            Application.Quit();
        }
    }
    public void OnClick_GO_Retry()
    {
        GameOver(false); 
        
        StartChangeScene(SCENE_INDEX.GAMESTAGE, true);
        if (onClickGameOverMenu != null) onClickGameOverMenu(MENU_GAMEOVER.RETRY);
    }
    public void OnClick_GO_ReviveShards()
    {
        if (onClickGameOverMenu != null) onClickGameOverMenu(MENU_GAMEOVER.REVIVE_SHARDS);
    }
    public void OnClick_GO_ReviveAd()
    {
        if (onClickGameOverMenu != null) onClickGameOverMenu(MENU_GAMEOVER.REVIVE_AD);
    }
    #endregion

    #region ### Upgrade UI ###

    [Header("- Upgrade UI -")]
    public TweenTransforms upgradeUITweener;
    public List<UpgradeElement> upgradeElements;
    public GameObject ReturnButton;

    private Vector3 outXPos_upgradeUI = new Vector3(900, 225, 0);
    private Vector3 inXPos_upgradeUI = new Vector3(0, 225, 0);

    public bool isShowingUpgradeUI = false;

    public System.Action onClickReturn;

    public void ShowUpgradeUI(bool isShow)
    {
        if (isShow)
        {
            upgradeUITweener.gameObject.SetActive(true);

            upgradeUITweener.startingVector = outXPos_upgradeUI;
            upgradeUITweener.endVector = inXPos_upgradeUI;

            SetUpgradeInfos(upgradeElements);

            Vibration.Vibrate(100);
            SoundManager.StopAllLoopingSounds();
            if (onCameraShake != null) onCameraShake(5);
        }
        else
        {
            upgradeUITweener.startingVector = inXPos_upgradeUI;
            upgradeUITweener.endVector = outXPos_upgradeUI;

            upgradeUITweener.TweenCompleted += DeactiveUpgradeUIAfterTweenComplete;

            currentGameUIStatus = TOPUI_STATUS.NORMAL;
        }

        isShowingUpgradeUI = isShow;
        upgradeUITweener.Begin();
        upgradeUITweener.defaultVector = upgradeUITweener.startingVector;
    }

    public void DeactiveUpgradeUIAfterTweenComplete()
    {
        upgradeUITweener.TweenCompleted -= DeactiveUpgradeUIAfterTweenComplete;
        upgradeUITweener.gameObject.SetActive(false);
    }

    void SetUpgradeInfos(List<UpgradeElement> upgradeLists)
    {
        foreach (UpgradeElement element in upgradeElements)
        {
            element.SetInfo();
            element.onClickUpgradeButton = OnClickUpgradeButton;
        }
    }

    public void OnClickReturn()
    {
        if (onClickReturn != null) onClickReturn();
    }

    void OnClickUpgradeButton(SKILL_TYPE skillType)
    {
        // Upgrade Possible
        if ((GetRequiredShardsForUpgrade(skillType) <= PlayerManager.Instance().PlayerStatus.CurrentMemoryShards)
            && (currentGameUIStatus == TOPUI_STATUS.GAMEOVER))
        {
            int requiredShards = GetRequiredShardsForUpgrade(skillType);

            PlayerManager.Instance().PlayerStatus.CurrentMemoryShards -= requiredShards;
            GameConfigs.SetCurrentMemoryShards(PlayerManager.Instance().PlayerStatus.CurrentMemoryShards);

            GameConfigs.SetSkillLevel(skillType, GameConfigs.SkillLevel(skillType) + 1);

            foreach (UpgradeElement element in upgradeElements)
            {
                element.SetInfo();
            }


            int effectShardAmount = (GameConfigs.SkillLevel(skillType) / 3) + 1;

            if (effectShardAmount < 1) effectShardAmount = 1;
            if (effectShardAmount > 10) effectShardAmount = 10;

            StageLoader.Instance().Generate_SkillUpgradeEffectShards(skillType, effectShardAmount);
            
            Vibration.Vibrate(3);
            StartGlobalLightEffect(Color.white, 1f, 0.2f);
            SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_upgrade], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_upgrade].clip);

            if (onCameraShake != null) onCameraShake(2);
        }
    }

    #endregion

    #region ### Portal Purchase UI ###

    [Header("- Portal Purchase UI -")]

    public bool IsShowingPortalPurchaseUI = false;
    public bool IsCollidingPortalPurchase = false;

    public TweenTransforms portalPurchaseTweenTrans;
    public UnityEngine.UI.Text shardsAmountsToOpenPortal;

    public GameObject NoPortalPurchasePanel;

    private Vector3 outXPos_portalPurchaseUI = new Vector3(-900, -549, 0);
    private Vector3 inXPos_portalPurchaseUI = new Vector3(-293, -549, 0);

    public MoveTrigger currentSelectedPortalTrigger;

    public void ShowPortalPurchaseUI(bool isShow, MoveTrigger moveTrigger = null)
    {
        if (isShow && (moveTrigger != null))
        {
            // Show PortalPurchase
            portalPurchaseTweenTrans.gameObject.SetActive(true);

            //portalPurchaseTweenTrans.startingVector = portalPurchaseTweenTrans.transform.localPosition;
            //portalPurchaseTweenTrans.endVector = inXPos_portalPurchaseUI;

            portalPurchaseTweenTrans.gameObject.transform.localPosition = inXPos_portalPurchaseUI;

            shardsAmountsToOpenPortal.text = GameStatics.GetPortalOpenCost(moveTrigger.portalType).ToString();
            CheckPortalPurchaseCostBlock(moveTrigger.portalType);

            currentSelectedPortalTrigger = moveTrigger;
        }
        else
        {
            portalPurchaseTweenTrans.startingVector = portalPurchaseTweenTrans.transform.localPosition;
            portalPurchaseTweenTrans.endVector = outXPos_portalPurchaseUI;

            NoPortalPurchasePanel.SetActive(true);
            currentSelectedPortalTrigger = null;

            portalPurchaseTweenTrans.duration = 0.4f;
            portalPurchaseTweenTrans.Begin();
            portalPurchaseTweenTrans.defaultVector = portalPurchaseTweenTrans.startingVector;
        }


        IsShowingPortalPurchaseUI = isShow;
    }

    void CheckPortalPurchaseCostBlock(PORTAL_TYPE portalType)
    {
        if (GameStatics.GetPortalOpenCost(portalType) < PlayerManager.Instance().PlayerStatus.CurrentMemoryShards)
        {
            // Pucharsable
            NoPortalPurchasePanel.SetActive(false);
        }
        else
        {
            NoPortalPurchasePanel.SetActive(true);
        }
    }

    public void OnClickPortalPurchase()
    {
        if (currentSelectedPortalTrigger != null)
        {
            if (GameStatics.GetPortalOpenCost(currentSelectedPortalTrigger.portalType) <= PlayerManager.Instance().PlayerStatus.CurrentMemoryShards)
            {
                // Open the portal
                PlayerManager.Instance().PlayerStatus.CurrentMemoryShards -= GameStatics.GetPortalOpenCost(currentSelectedPortalTrigger.portalType);
                GameConfigs.SetPortalStatus(currentSelectedPortalTrigger.portalType, true);
                GameConfigs.SetCurrentMemoryShards(PlayerManager.Instance().PlayerStatus.CurrentMemoryShards);

                currentSelectedPortalTrigger.SetPortal(true);

                StartGlobalLightEffect(Color.magenta, 4f, 0.6f);
                SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_upgrade], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_upgrade].clip);
                if (onCameraShake != null) onCameraShake(7);
            }
        }
    }

    #endregion

    #region ### Global Light Effect ###

    [Header("- Top Light Effect -")]

    public Light2D globalLightEffect;
    public TweenValue globalLightIntensity;

    public bool isGlobalLighting = false;

    #endregion

    private void Update()
    {
        if (isGlobalLighting)
        {
            globalLightEffect.intensity = globalLightIntensity.value;
        }


        if (IsCollidingPortalPurchase == false && IsShowingPortalPurchaseUI == true)
        {
            ShowPortalPurchaseUI(false, null);
        }
    }

    public void StartGlobalLightEffect(Color lightColor, float lightIntensity, float oneWaySpeed)
    {
        if (isGlobalLighting == false)
        {
            globalLightEffect.color = lightColor;
            globalLightIntensity.myTweenType = TweenBase.playStyles.PingPongOnce;
            globalLightIntensity.startValue = 0;
            globalLightIntensity.endValue = lightIntensity;
            globalLightIntensity.duration = oneWaySpeed;

            isGlobalLighting = true;

            BrightOnLight();
            StartCoroutine(GlobalLightTimer(oneWaySpeed * 2));
        }
    }

    void BrightOnLight()
    {
        globalLightIntensity.Begin();
        globalLightIntensity.value = globalLightIntensity.startValue;
    }

    IEnumerator GlobalLightTimer(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        globalLightEffect.intensity = 0;
        isGlobalLighting = false;
    }

    #region #### SubTitle Control ####

    public GameObject[] SubTitles;

    public void ShowSubTitle(int stage)
    {
        if (SubTitles.Length < stage) return;

        int targetIndex = stage - 1;
        for (int i = 0; i < SubTitles.Length; i++)
        {
            if (i == targetIndex)
            {
                SubTitles[i].SetActive(true);
                GameObject go = SubTitles[i];

                go.GetComponent<TweenTransforms>().Begin();
                go.GetComponent<TweenAlpha>().Begin();
            }
            else SubTitles[i].SetActive(false);
        }
    }

    #endregion

    #region #### Setting Control ####

    [Header("- Setting Control -")]

    public GameObject SettingShowButton;
    public UnityEngine.UI.Text SettingGoBackText; 
    public TweenTransforms SettingControlTweenTrans;

    private Vector3 outXPos_SettingControl = new Vector3(-900, 0, 0);
    private Vector3 inXPos_SettingControl = new Vector3(0, 0, 0);

    public bool IsSettingState = false;

    public void OnClickSettingShow()
    {
        bool possibleSettingShow = false;

        if (SceneManager.GetActiveScene().buildIndex == (int)SCENE_INDEX.MAINMENU) possibleSettingShow = true;
        else if ((SceneManager.GetActiveScene().buildIndex == (int)SCENE_INDEX.GAMESTAGE) && PlayerManager.Instance().GetPlayerControl().IsGround()) possibleSettingShow = true;

        if (possibleSettingShow)
        {
            topCanvas.sortingOrder = 10;

            SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select].clip);

            IsSettingState = true;
            SettingControlTweenTrans.gameObject.SetActive(true);
            SettingControlTweenTrans.startingVector = outXPos_SettingControl;
            SettingControlTweenTrans.endVector = inXPos_SettingControl;

            if (SceneManager.GetActiveScene().buildIndex == (int)SCENE_INDEX.MAINMENU)
            {
                SettingGoBackText.text = "Exit Game";
            }
            else
            {
                SettingGoBackText.text = "Main Menu";
            }

            SettingControlTweenTrans.Begin();
            SettingControlTweenTrans.defaultVector = SettingControlTweenTrans.startingVector;
        }
    }

    public void OnClickSettingHide()
    {
        IsSettingState = false;
        SettingControlTweenTrans.startingVector = inXPos_SettingControl;
        SettingControlTweenTrans.endVector = outXPos_SettingControl;
        SettingControlTweenTrans.TweenCompleted += SettingControlFadeoutFinish;

        SettingControlTweenTrans.Begin();
        SettingControlTweenTrans.defaultVector = SettingControlTweenTrans.startingVector;
    }

    void SettingControlFadeoutFinish()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SCENE_INDEX.MAINMENU) topCanvas.sortingOrder = 0;
        SettingControlTweenTrans.TweenCompleted -= SettingControlFadeoutFinish;
    }

    public void OnChangePlayerStatus()
    {
        if (isShowingUpgradeUI)
        {
            // Update Upgrade UIs
            SetUpgradeInfos(upgradeElements);
        }

        if (isShowingSecondWind)
        {
            // Update Second Wind UIs

            if (currentRequiredShardsAmounts > PlayerManager.Instance().PlayerStatus.CurrentMemoryShards)
            {
                NoMoreRevivePanel.SetActive(true);
            }
            else
            {
                NoMoreRevivePanel.SetActive(false);
            }
        }
    }

    #endregion

    [Header("- TEST -")]
    public TestScript testscript;
}
