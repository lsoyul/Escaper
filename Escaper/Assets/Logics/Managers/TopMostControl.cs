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

        if (scene.buildIndex == (int)SCENE_INDEX.GAMING) {
            StageLoader.Instance().SetStage(1);
        } 
        else {
            StageLoader.Instance().DisableStage();
        }
    }

    public void StartChangeScene(SCENE_INDEX targetSceneIndex, bool smoothChange)
    {
        isChangingState = true;
        blackPanelAlpha.startValue = 0f;
        blackPanelAlpha.endValue = 1f;
        currentTargetScene = targetSceneIndex;
        blackPanelAlpha.TweenCompleted += SceneFadeoutFinishEvent;
        blackPanelAlpha.Begin();
        blackPanelAlpha.value = blackPanelAlpha.startValue;
    }

    void SceneFadeoutFinishEvent()
    {
        if (isChangingState)
        {
            SceneManager.LoadSceneAsync((int)currentTargetScene, LoadSceneMode.Single);
            blackPanelAlpha.TweenCompleted -= SceneFadeoutFinishEvent;
        }
    }
}
