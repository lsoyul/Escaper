using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using System;
using DigitalRuby.SoundManagerNamespace;
using static GameStatics;

public class PlayerManager : MonoBehaviour
{
    private static GameObject container;
    private static PlayerManager instance;
    public static PlayerManager Instance()
    {
        //if (instance == null)
        //{
        //    container = new GameObject();
        //    container.name = "PlayerManager";
        //    instance = container.AddComponent(typeof(PlayerManager)) as PlayerManager;
        //}

        return instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            container = this.gameObject;
            container.name = "PlayerManager";   
            instance = GetComponent<PlayerManager>();
            DontDestroyOnLoad(this);
        }

        SetPlayerController(false);

        if (playerStatus == null) playerStatus = new PlayerStatus();

        playerStatus.onChangePlayerStatus += OnChangePlayerStatus;

        TopMostControl.Instance().onClickGameOverMenu += OnClickGameOverMenu;
        onChangePlayerStatus += TopMostControl.Instance().OnChangePlayerStatus;
    }

    public static bool HasInstance()
    {
        return instance;
    }

    #region ### Camera ###

    CameraControlScript cameraController;

    public void SetCameraController(CameraControlScript cam)
    {
        cameraController = cam;
    }

    public CameraControlScript CameraController()
    {
        return cameraController;
    }

    #endregion

    #region ### Player ###

    public PlayerControllerScripts playerController;
    private PlayerStatus playerStatus = new PlayerStatus();


    public PlayerControllerScripts GetPlayerControl()
    {
        return this.playerController;
    }

    public void SetPlayerController(bool active, bool isRightInit = true)
    {
        if (playerController.transform.parent != this.transform)
        {
            playerController.transform.parent = this.transform;
        }

        playerController.gameObject.SetActive(active);

        if (active)
        {
            playerController.transform.position = StageLoader.Instance().GetCurrentStage().GetPlayerInitPos().position;

            Vector3 tVec = playerController.playerAnimator.transform.localScale;
            if ((isRightInit && tVec.x < 0)
                || (!isRightInit && tVec.x > 0))
            {
                tVec.x *= -1;
                playerController.playerAnimator.transform.localScale = tVec;
            }
        }
    }

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

    public Action onFinishAirTime;


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
        //if (IsDead)
        //{
        //    if (playerStatus.CurrentHP > 0)
        //    {
        //        // Revive
        //        IsDead = false;
        //
        //    }
        //}

        if (playerStatus.CurrentHP > 0)
        {
            if (playerController.startDeathForUpgradeAct == true) playerController.startDeathForUpgradeAct = false;
            if (IsDead) IsDead = false;
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
        playerStatus.CurrentHP -= GameStatics.GetDamagePoints(damageType);
        playerController.ShowDamageText(GameStatics.GetDamagePoints(damageType));
        switch (damageType)
        {
            case DAMAGED_TYPE.SPIKE:
                TopMostControl.Instance().StartGlobalLightEffect(Color.red, 1f, 0.2f);
                SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_hitWall], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_hitWall].clip);
                break;
            case DAMAGED_TYPE.FALLING_GROUND:
                TopMostControl.Instance().StartGlobalLightEffect(Color.red, 2f, 0.4f);
                SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_fallGround], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_fallGround].clip);
                break;
            case DAMAGED_TYPE.PROJECTILE_SHOOTER1:
                TopMostControl.Instance().StartGlobalLightEffect(Color.red, 1f, 0.2f);
                SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_hitWall], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_hitWall].clip);
                break;
            default:
                break;
        }

        if (onDamaged != null) onDamaged(damageType);

    }

    public void OnGround()
    {
        // If player grounded,
        SetAirTimeFinish();

        if (IsDead)
        {
            // GameOver Check (by check current hp)
            TopMostControl.Instance().GameOver(true);
        }
        else
        {
            SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_jumpStomp], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_jumpStomp].clip);
        }
    }

    void OnClickGameOverMenu(MENU_GAMEOVER gameOverMenu)
    {
        switch (gameOverMenu)
        {
            case MENU_GAMEOVER.MAINMENU:
                // Save Memory Shards..
                GameConfigs.SetCurrentMemoryShards(PlayerStatus.CurrentMemoryShards);
                SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select].clip);
                break;
            case MENU_GAMEOVER.RETRY:
                // Save Memory Shards..
                GameConfigs.SetCurrentMemoryShards(PlayerStatus.CurrentMemoryShards);
                SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_select].clip);
                break;
            case MENU_GAMEOVER.REVIVE_SHARDS:

                if (PlayerStatus.RemainReviveCount > 0)
                {
                    Vibration.Vibrate(100);
                    PlayerStatus.CurrentMemoryShards -= TopMostControl.Instance().GetRequiredShardsForRevive();
                    PlayerStatus.CurrentHP = PlayerStatus.MaxHP;
                    PlayerStatus.RemainReviveCount -= 1;

                    TopMostControl.Instance().StartGlobalLightEffect(Color.yellow, 2f, 0.2f);
                    TopMostControl.Instance().GameOver(false);

                    if (cameraController != null)
                    {
                        cameraController.CameraShake_Rot(5);
                    }

                    EffectManager.GetInstance().playEffect(playerController.GetPlayerRigidBody().transform.position, EFFECT.YELLOW_PILLAR, Vector2.zero);
                    TopMostControl.Instance().StartBGM(SceneManager.GetActiveScene());
                    SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_revive], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_revive].clip);
                }
                break;
            case MENU_GAMEOVER.REVIVE_AD:
                if (PlayerStatus.RemainReviveCount > 0)
                {
                    Vibration.Vibrate(100);
                    PlayerStatus.CurrentHP = PlayerStatus.MaxHP;
                    PlayerStatus.RemainReviveCount -= 1;

                    TopMostControl.Instance().StartGlobalLightEffect(Color.yellow, 2f, 0.2f);
                    TopMostControl.Instance().GameOver(false);

                    if (cameraController != null)
                    {
                        cameraController.CameraShake_Rot(5);
                    }

                    EffectManager.GetInstance().playEffect(playerController.GetPlayerRigidBody().transform.position, EFFECT.YELLOW_PILLAR, Vector2.zero);
                    TopMostControl.Instance().StartBGM(SceneManager.GetActiveScene());
                    SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_revive], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_revive].clip);
                }
                break;
            default:
                break;
        }
    }

    void OnClickReturnButton()
    {
        // Death for upgrade hero

    }

    #region ### Shard get callback ###


    public void OnGetShard(MemoryShard shard)
    {
        if (shard.GetShardType() == SHARD_TYPE.SHARD1)
        {
            PlayerStatus.CurrentMemoryShards +=
                (int)(StageLoader.Instance().GetRandomShard(shard.GetShardType()) * StageLoader.Instance().GetShardMultifly());

            TopMostControl.Instance().StartGlobalLightEffect(shard.GetShardTargetColor(), 0.3f, 0.1f);
        }

        GetPlayerControl().BlinkPlayerShardLight(shard.GetShardTargetColor());
        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_gainShard], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_gainShard].clip);
    }

    #endregion

    #region ### Skill Controls ###


    private double airTimeTimer = 0f;
    private bool isAirTime = false;

    IEnumerator AirTime()
    {
        isAirTime = true;
        airTimeTimer = 0f;

        while (airTimeTimer < PlayerStatus.AirTimeDuration)
        {
            airTimeTimer += Time.unscaledDeltaTime;
            yield return null;
        }

        airTimeTimer = 0;

        isAirTime = false;

        if (onFinishAirTime != null) onFinishAirTime();
    }

    public void Skill_AirTime()
    {
        StartCoroutine(AirTime());
    }

    public bool GetIsAirTime()
    {
        return isAirTime;
    }

    public double GetAirTimeTimer()
    {
        return airTimeTimer;
    }

    public void SetAirTimeFinish()
    {
        airTimeTimer = PlayerStatus.AirTimeDuration;
    }

    #endregion



}
