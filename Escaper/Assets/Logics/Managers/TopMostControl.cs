using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using static GameStatics;

public class TopMostControl : MonoBehaviour
{
    public GameObject topMostBlackPanel;
    public TweenAlpha blackPanelAlpha;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isChangingState = false;
        blackPanelAlpha.startValue = 1f;
        blackPanelAlpha.endValue = 0f;
        blackPanelAlpha.Begin();
        blackPanelAlpha.value = blackPanelAlpha.startValue;

        if (scene.buildIndex == (int)SCENE_INDEX.GAMESTAGE)
            StageLoader.Instance().SetStage(StageLoader.NextStage);
        else
            StageLoader.Instance().DisableStage();
        
    }

    public void StartChangeScene(SCENE_INDEX targetSceneIndex, bool smoothChange, int nextStageNum = 1)
    {
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

    private float outXPos_gameOverUI = 900;
    private float inXPos_gameOverUI = 0;

    public System.Action<MENU_GAMEOVER> onClickGameOverMenu;

    public void ShowGameOver(bool isShow)
    {
        if (isShow)
        {
            int shardsAmounts = PlayerManager.Instance().PlayerStatus.CurrentMemoryShards / 2;
            shardsAmountsToRevive.text = "-"+shardsAmounts.ToString();
            gameOverTweenTrans.startingVector.x = outXPos_gameOverUI;
            gameOverTweenTrans.endVector.x = inXPos_gameOverUI;
        }
        else
        {
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
