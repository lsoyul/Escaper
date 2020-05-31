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

        GameManager.Instance().onUserEarnedReward_Revive += OnUserEarnedReward_Revive;
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
        SetAirTimeFinish();
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
            case DAMAGED_TYPE.PROJECTILE_SHOOTER2:
                TopMostControl.Instance().StartGlobalLightEffect(Color.red, 1f, 0.2f);
                SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_hitWall], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_hitWall].clip);
                break;
            case DAMAGED_TYPE.EARTH_QUAKE:
                    TopMostControl.Instance().StartGlobalLightEffect(Color.red, 2f, 0.4f);
                    SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_fallGround], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_fallGround].clip);
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
                        cameraController.CameraShake_Rot(3);
                    }

                    EffectManager.GetInstance().playEffect(playerController.GetPlayerRigidBody().transform.position, EFFECT.YELLOW_PILLAR, Vector2.zero);
                    TopMostControl.Instance().StartBGM(SceneManager.GetActiveScene());
                    SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_revive], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_revive].clip);

                    Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_REVIVE_SHARD);
                }
                break;
            case MENU_GAMEOVER.REVIVE_AD:

                GameManager.Instance().ShowReviveAds();
                break;
            default:
                break;
        }
    }

    void OnUserEarnedReward_Revive()
    {
        // Success View Ads for Revive
        if (PlayerStatus.RemainReviveCount > 0)
        {
            Vibration.Vibrate(100);
            PlayerStatus.CurrentHP = PlayerStatus.MaxHP;
            PlayerStatus.RemainReviveCount -= 1;

            TopMostControl.Instance().StartGlobalLightEffect(Color.yellow, 2f, 0.2f);
            TopMostControl.Instance().GameOver(false);

            if (cameraController != null)
            {
                cameraController.CameraShake_Rot(3);
            }

            EffectManager.GetInstance().playEffect(playerController.GetPlayerRigidBody().transform.position, EFFECT.YELLOW_PILLAR, Vector2.zero);
            TopMostControl.Instance().StartBGM(SceneManager.GetActiveScene());
            SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_revive], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_revive].clip);

            Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_REVIVE_ADS);
        }
    }

    void OnClickReturnButton()
    {
        // Death for upgrade hero

    }

    #region ### Shard get callback ###


    public void OnGetShard(MemoryShard shard)
    {
        if (shard.GetShardType() == SHARD_TYPE.SHARD1
            || shard.GetShardType() == SHARD_TYPE.SHARD2)
        {
            int resultShardAmount = (int)(StageLoader.Instance().GetRandomShard(shard.GetShardType()) * StageLoader.Instance().GetShardMultifly());

            PlayerStatus.CurrentMemoryShards += resultShardAmount;
            playerController.ShowShardGetText(resultShardAmount);
        }

        TopMostControl.Instance().StartGlobalLightEffect(shard.GetShardTargetColor(), 0.5f, 0.1f);
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

    #region ### Hero Sprite Control ###

    private PLAYER_SKIN currentPlayerSprite = PLAYER_SKIN.NORMAL;

    public List<PLAYER_SKIN> PlayerHaveSkinList = new List<PLAYER_SKIN>();

    public PLAYER_SKIN CurrentPlayerSprite
    {
        get { return currentPlayerSprite; }
    }

    public Action onChangePlayerSkin;

    public void SetPlayerSkin(PLAYER_SKIN playerSkin)
    {
        currentPlayerSprite = playerSkin;
        if (onChangePlayerSkin != null) onChangePlayerSkin();
    }

    #region #### Ending Trigger ####

    private GameStatics.PLAY_MODE playMode;

    public GameStatics.PLAY_MODE PlayMode
    {
        get { return playMode; }
        set { playMode = value; }
    }


    private bool isTriggerEnding;
    public Action onTriggerEnding;

    public bool IsTriggerEnding
    {
        get { return isTriggerEnding; }
        set { 
            isTriggerEnding = value; 
            if (isTriggerEnding == true)
            {
                if (PlayMode == PLAY_MODE.NORMAL)
                {
                    GameConfigs.SetNormalEnding();
                    Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_TRIGGER_ENDING_NORMAL);
                    PlayMode = PLAY_MODE.TRUE;
                }
                else if (PlayMode == PLAY_MODE.TRUE)
                {
                    GameConfigs.SetTrueEnding();
                    Firebase.Analytics.FirebaseAnalytics.LogEvent(GameStatics.EVENT_TRIGGER_ENDING_TRUEHERO);
                }

                StartCoroutine(ShowEnding());
                if (onTriggerEnding != null) onTriggerEnding();
            }
        }
    }


    #endregion

    IEnumerator ShowEnding()
    {
        TopMostControl.Instance().SettingShowButton.SetActive(false);
        TopMostControl.Instance().ReturnButton.SetActive(false);

        SoundManager.StopAllLoopingSounds();

        TimeSpan curPlayTimeSpan = TimeSpan.FromSeconds(TopMostControl.Instance().playUnixTime);
        string playTimeText
            = string.Format("{0}:{1}:{2}", curPlayTimeSpan.Hours, curPlayTimeSpan.Minutes.ToString("D2"), curPlayTimeSpan.Seconds.ToString("D2"));

        yield return new WaitForSeconds(1f);
        // 1. Play Victory Sound
        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_victory], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_victory].clip);

        yield return new WaitForSeconds(3f);

        // 2. Show Confetti Effect
        Vector3 blast1 = cameraController.GetTargetPos();
        blast1.y += 60f;

        Vector3 blast2 = cameraController.GetTargetPos();
        blast2.y += 50f;
        blast2.x -= 20f;

        Vector3 blast3 = cameraController.GetTargetPos();
        blast3.y += 70f;
        blast3.x += 20f;

        Vector3 directional_left = cameraController.GetTargetPos();
        directional_left.y += 150f;
        directional_left.x -= 90f;

        Vector3 directional_right = cameraController.GetTargetPos();
        directional_right.y += 150f;
        directional_right.x += 90f;

        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1].clip);
        EffectManager.GetInstance().playEffect(directional_left, EFFECT.CONFETTI_DIRECTIONAL, Vector2.zero, false);
        yield return new WaitForSeconds(0.5f);
        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1].clip);
        EffectManager.GetInstance().playEffect(directional_right, EFFECT.CONFETTI_DIRECTIONAL, Vector2.zero, true);
        yield return new WaitForSeconds(1f);
        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1].clip);
        EffectManager.GetInstance().playEffect(blast1, EFFECT.CONFETTI_BLAST, Vector2.zero);
        yield return new WaitForSeconds(0.3f);
        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1].clip);
        EffectManager.GetInstance().playEffect(blast2, EFFECT.CONFETTI_BLAST, Vector2.zero);
        yield return new WaitForSeconds(0.3f);
        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_explosion1].clip);
        EffectManager.GetInstance().playEffect(blast3, EFFECT.CONFETTI_BLAST, Vector2.zero);

        yield return new WaitForSeconds(3f);

        // 3. Show Records
        TopMostControl.Instance().PopupSingle.ShowPopup(
            "<color=yellow>Contraturation !</color>",
            "<color=white>PlayTime:</color> " + "<color=red>" + playTimeText + "</color>");

        TopMostControl.Instance().PopupSingle.ShowPopup(
            "<color=yellow>CLEAR Normal Mode</color>",
            "<color=red>True Hero Mode</color>\n <color=white>is Unlocked!</color>",
            () => {
                TopMostControl.Instance().StartChangeScene(SCENE_INDEX.MAINMENU, true);
            });
    }

    //=== TESTCODE ===

    [Header(" -- TEST CODE -- ")]
    public PLAYER_SKIN SKIN_FORTEST;

    [ContextMenu("ChangeSkin")]
    void TEST_ChangeSkin()
    {
        SetPlayerSkin(SKIN_FORTEST);
    }

    public void TEST_SetRandomSkin()
    {
        int randomIndex = UnityEngine.Random.Range(0, 7);
        SetPlayerSkin((PLAYER_SKIN)randomIndex);
    }

    //================

    #endregion

}
