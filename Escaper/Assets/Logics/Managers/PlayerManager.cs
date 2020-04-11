using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using static GameStatics;

public class PlayerManager : MonoBehaviour
{
    private static GameObject container;
    private static PlayerManager instance;
    public static PlayerManager Instance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "PlayerManager";
            instance = container.AddComponent(typeof(PlayerManager)) as PlayerManager;
        }

        return instance;
    }

    public static bool HasInstance()
    {
        return instance;
    }

    #region ### Player ###

    private PlayerStatus playerStatus = new PlayerStatus();

    public PlayerStatus PlayerStatus
    {
        get { return playerStatus; }
    }

    private bool isDead = false;

    public bool IsDead
    {
        get { return isDead; }
        set
        {
            isDead = value;
            if (onDeath != null) onDeath(isDead);
        }
    }

    #endregion

    public Action<DAMAGED_TYPE> onDamaged;
    public Action onChangePlayerStatus;
    public Action<bool> onDeath;

    private void Awake()
    {
        if(instance == null)
        {
            instance = GetComponent<PlayerManager>();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (playerStatus == null) playerStatus = new PlayerStatus();

        playerStatus.onChangePlayerStatus += OnChangePlayerStatus;

        TopMostControl.Instance().onClickGameOverMenu += OnClickGameOverMenu;
    }

    private void OnDestroy()
    {
        if (playerStatus != null)
        {
            playerStatus.onChangePlayerStatus -= OnChangePlayerStatus;
        }

        if (TopMostControl.HasInstance())
        {
            TopMostControl.Instance().onClickGameOverMenu -= OnClickGameOverMenu;
        }
    }

    void OnChangePlayerStatus()
    {
        if (IsDead)
        {
            if (playerStatus.CurrentHP > 0)
            {
                // Revive
                IsDead = false;
            }
        }

        if (playerStatus.CurrentHP <= 0)
        {
            IsDead = true;
        }

        if (onChangePlayerStatus != null) onChangePlayerStatus();
    }

    public void InitializePlayer()
    {
        if (playerStatus == null) playerStatus = new PlayerStatus();

        playerStatus.InitPlayerForGameStart();
    }

    public void OnDamaged(DAMAGED_TYPE damageType)
    {
        switch (damageType)
        {
            case DAMAGED_TYPE.SPIKE:
                playerStatus.CurrentHP -= GameStatics.DMG_SPIKE_BASIC;
                break;
            default:
                break;
        }

        if (onDamaged != null) onDamaged(damageType);

    }

    public void OnGround()
    {
        // If player grounded,

        if (IsDead)
        {
            // GameOver Check (by check current hp)
            TopMostControl.Instance().ShowGameOver(true);
        }
    }

    void OnClickGameOverMenu(MENU_GAMEOVER gameOverMenu)
    {
        switch (gameOverMenu)
        {
            case MENU_GAMEOVER.MAINMENU:
                // Save Memory Shards..
                GameConfigs.SetCurrentMemoryShards(PlayerStatus.CurrentMemoryShards);
                TopMostControl.Instance().ShowGameOver(false);
                TopMostControl.Instance().StartChangeScene(SCENE_INDEX.MAINMENU, true);
                break;
            case MENU_GAMEOVER.RETRY:
                // Save Memory Shards..
                GameConfigs.SetCurrentMemoryShards(PlayerStatus.CurrentMemoryShards);
                TopMostControl.Instance().ShowGameOver(false);
                TopMostControl.Instance().StartChangeScene(SCENE_INDEX.GAMESTAGE, true, StageLoader.CurrentStage);
                break;
            case MENU_GAMEOVER.REVIVE_SHARDS:
                PlayerStatus.CurrentMemoryShards = PlayerStatus.CurrentMemoryShards / 2;
                PlayerStatus.CurrentHP = PlayerStatus.MaxHP / 2;
                TopMostControl.Instance().ShowGameOver(false);
                break;
            case MENU_GAMEOVER.REVIVE_AD:
                PlayerStatus.CurrentHP = PlayerStatus.MaxHP;
                TopMostControl.Instance().ShowGameOver(false);
                break;
            default:
                break;
        }
    }
}
