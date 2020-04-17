using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
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


    private static GameObject container;
    private static TopMostControl instance;
    public static TopMostControl Instance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "TopMostControl";
            instance = container.AddComponent(typeof(TopMostControl)) as TopMostControl;
        }

        return instance;
    }

    public static bool HasInstance()
    {
        return instance;
    }

    private void Awake() {
        
        if (instance == null) 
        {
            instance = GetComponent<TopMostControl>();
            DontDestroyOnLoad(this);

            gameControlObj.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
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
        blackPanelAlpha.Begin();
        blackPanelAlpha.value = blackPanelAlpha.startValue;

        if (scene.buildIndex == (int)SCENE_INDEX.GAMESTAGE)
        {
            StageLoader.Instance().SetStage(StageLoader.NextStage);
            gameControlObj.SetActive(true);
            topCanvas.sortingOrder = 10;
        }
        else
        {
            StageLoader.Instance().DisableStage();
            gameControlObj.SetActive(false);
            topCanvas.sortingOrder = 0;
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
        }
    }

    public bool GetIsSceneChanging()
    {
        return isChangingState;
    }

    #region ### GameOverUI ###

    [Header("- GameOverUI -")]
    public TweenTransforms gameOverTweenTrans;
    public UnityEngine.UI.Text shardsAmountsToRevive;

    public GameObject NoMoreRevivePanel;

    private float outXPos_gameOverUI = 900;
    private float inXPos_gameOverUI = 0;

    public System.Action<MENU_GAMEOVER> onClickGameOverMenu;

    public void ShowGameOver(bool isShow)
    {
        if (PlayerManager.Instance().PlayerStatus.RemainReviveCount > 0)
        {
            NoMoreRevivePanel.SetActive(false);
        }
        else
        {
            NoMoreRevivePanel.SetActive(true);
        }

        if (isShow)
        {
            gameControlObj.SetActive(false);

            int shardsAmounts = PlayerManager.Instance().PlayerStatus.CurrentMemoryShards / 2;
            shardsAmountsToRevive.text = "-"+shardsAmounts.ToString();
            gameOverTweenTrans.startingVector.x = outXPos_gameOverUI;
            gameOverTweenTrans.endVector.x = inXPos_gameOverUI;
        }
        else
        {
            gameControlObj.SetActive(true);

            gameOverTweenTrans.startingVector.x = inXPos_gameOverUI;
            gameOverTweenTrans.endVector.x = outXPos_gameOverUI;
        }

        gameOverTweenTrans.Begin();
        gameOverTweenTrans.defaultVector = gameOverTweenTrans.startingVector;
    }

    public void OnClick_GO_MainMenu()
    {
        if (onClickGameOverMenu != null) onClickGameOverMenu(MENU_GAMEOVER.MAINMENU);
    }
    public void OnClick_GO_Retry()
    {
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
}
