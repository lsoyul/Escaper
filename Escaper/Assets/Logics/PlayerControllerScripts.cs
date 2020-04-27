using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using static GameStatics;

using DigitalRuby.SoundManagerNamespace;
public class PlayerControllerScripts : MonoBehaviour
{
    public FlickController flickController;
    public GameObject jumpGuide;
    public Rigidbody2D playerRigidbody2D;
    public EdgeCollider2D groundEdgeCollider;
    public Transform groundCheckCenter;
    public float timescale = 1.0f;
    bool isSlowMode = false;
    private int possibleMaxJump = 2;
    [SerializeField] private int currentRemainJump = 2;

    public float minimumYSpeed = -500f;

    public Animator playerAnimator;
    public float characterScale;

    [Header("- Effects -")]
    public Animator glintAnimation;
    public LightBlink shardLight;

    public Color ColorBlink_AirJump;
    public Color ColorBlink_Shard;

    // Ground Check
    [SerializeField] bool isGround = false;
    [SerializeField] bool isAirJump = false;

    [Header("- Sprite Renderer -")]
    public SpriteRenderer playerRenderer;
    private float initFixedDeltaTime = 0f;
    [SerializeField] private float slowDownTimescale = 0.1f;

    [SerializeField] private Collider2D groundCollideObject = null;

    [Header("- Player Hurting Status -")]
    public bool canTriggerHurt = true;
    public bool reviveTime = false;
    public float unbeatableDuration_hurt = 3.0f;
    public float unbeatableDuration_revive = 5.0f;
    private bool isHurting = false;
    public float knockbackXPower = 50f;
    public float knockbackYPower = 50f;
    public LightBlink damagedLight;
    public LightBlink reviveLight;

    public TMPro.TextMeshPro textDamagedObject;
    public TweenValue textDamagedAlpha;
    public TweenTransforms textDamagedTransform;

    // Falling State
    public bool isFalling = false;
    public bool isFainting = false;

    // Death for upgrade
    public bool startDeathForUpgradeAct = false;


    [Header("- Player AirJump Effect -")]
    public LightBlink airJumpLight;
    public GameObject skillCooltime;

    [Header("- ForTest -")]
    public Transform initPos;


    private void Start()
    {
        if (flickController == null)
        {
            flickController = TopMostControl.Instance().GetController();

            if (flickController == null) Debug.LogError("FlickController is null");
        }

        characterScale = playerAnimator.transform.localScale.x;

        flickController.onPointerUp += OnPointerUp;
        flickController.onPointerDown += OnPointerDown;

        PlayerManager.Instance().onDeath += OnDeath;
        PlayerManager.Instance().onFinishAirTime += OnFinishAirTime;

        TopMostControl.Instance().onClickReturn += OnClickReturnButton;
        TopMostControl.Instance().onClickGameOverMenu += OnClickGameOverMenu;

        initFixedDeltaTime = Time.fixedDeltaTime;

        EffectManager.GetInstance();
    }

    private void OnDestroy() {
        if (flickController != null)
        {
            flickController.onPointerUp -= OnPointerUp;
            flickController.onPointerDown -= OnPointerDown;
        }

        if (PlayerManager.HasInstance())
        {
            PlayerManager.Instance().onDeath -= OnDeath;
            PlayerManager.Instance().onFinishAirTime -= OnFinishAirTime;
        }

        if (TopMostControl.HasInstance())
        {
            TopMostControl.Instance().onClickReturn -= OnClickReturnButton;
            TopMostControl.Instance().onClickGameOverMenu -= OnClickGameOverMenu;
        }
    }

    private void Update() {
        Time.timeScale = timescale;
        if (timescale < 1.0f) Time.fixedDeltaTime = slowDownTimescale * timescale * GameStatics.fixedDeltaOffset;
        else Time.fixedDeltaTime = initFixedDeltaTime;

        bool beforeGround = isGround;

        isGround = CheckGround();

        // Touch Control
        if (flickController.GetIsHolding() && currentRemainJump > 0)
        {
            if (!isFalling && !isFainting && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("player_standup") && !startDeathForUpgradeAct)
            {
                Vector2 endPos = playerRigidbody2D.transform.position;
                endPos.x -= flickController.GetFlickedVector().x;
                endPos.y -= flickController.GetFlickedVector().y;

                Debug.DrawLine(playerRigidbody2D.transform.position, endPos, Color.yellow);

                if (flickController.GetFlickedVector().y < 0 && isGround == true)
                {
                    // Finger Flick to down at ground (ready to jump upside)
                    jumpGuide.SetActive(true);
                }
                else if (isGround == false)
                {
                    // When, ready to air jump in air
                    jumpGuide.SetActive(true);
                }
                else
                {
                    jumpGuide.SetActive(false);
                }

                Vector3 vec = playerRigidbody2D.position + -flickController.GetFlickedVector() * 0.3f;
                vec.z = -1f;
                jumpGuide.transform.position = vec;
            }
        }
        else
        {
            jumpGuide.SetActive(false);
            //Eff_jumpCharge.SetActive(false);
        }

        if (playerRigidbody2D.velocity.y < minimumYSpeed)
        {
            // FAlling state
            Vector2 t = playerRigidbody2D.velocity;
            t.y = minimumYSpeed;
            playerRigidbody2D.velocity = t;

            isFalling = true;
        }

        ControlGroundCollide();

        UpdateAnimator();

        // Grounded Just Timing Callback
        if (beforeGround == false)
        {
            if (isGround == true)
            {
                if (isFalling)
                {
                    damagedLight.StartBlink(1.0f, Color.red);
                    PlayerManager.Instance().OnDamaged(DAMAGED_TYPE.FALLING_GROUND);
                    isFainting = true;
                    isFalling = false;
                }

                PlayerManager.Instance().OnGround();
            }

        }
    }



    void OnPointerUp()
    {
        if (flickController.GetFlickedVector().magnitude > 1f && currentRemainJump > 0)
        {
            if (isGround == false) 
            {
                PlayerManager.Instance().SetAirTimeFinish();

                playerRigidbody2D.velocity = Vector2.zero;
                playerRigidbody2D.AddForce(-flickController.GetFlickedVector(), ForceMode2D.Impulse);

                isAirJump = true;
                Vibration.Vibrate(1);

                if (flickController.GetFlickedVector().magnitude >= 100f)
                {
                    // Double Jump
                    bool xReverse = (flickController.GetFlickedVector().x < 0) ? true : false; 

                    Vector3 effTargetPos = Vector3.zero;

                    EffectManager.GetInstance().playEffect(effTargetPos, GameStatics.EFFECT.JUMP_TWICE, flickController.GetFlickedVector(), xReverse, this.transform);
                    SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_doubleJump], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_doubleJump].clip);
                }
            }
            else if (isGround == true)
            {
                // Ground Jump!
                if (flickController.GetFlickedVector().y < 0 && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("player_standup"))
                {
                    playerRigidbody2D.velocity = Vector2.zero;
                    playerRigidbody2D.AddForce(-flickController.GetFlickedVector(), ForceMode2D.Impulse);

                    if (flickController.GetFlickedVector().magnitude >= 100f)
                    {
                        bool xReverse = (flickController.GetFlickedVector().x < 0) ? true : false; 

                        Vector3 effTargetPos = groundCheckCenter.position;
                        effTargetPos.y = groundCheckCenter.position.y + 7.3f;
                        
                        if (xReverse) effTargetPos.x = groundCheckCenter.position.x - 5f;
                        else effTargetPos.x = groundCheckCenter.position.x + 5f;

                        EffectManager.GetInstance().playEffect(effTargetPos, GameStatics.EFFECT.JUMP_SMOKE, Vector2.zero, xReverse);
                        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_jump], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_jump].clip);
                    }
                }
            }

        }

        airJumpLight.Hide();

        currentRemainJump -= 1;
        timescale = 1.0f;
    }

    void OnPointerDown()
    {
        if (!isFalling && !isFainting && !startDeathForUpgradeAct)
        {
            if (currentRemainJump > 0 && isGround == false && isHurting == false)
            {
                //#### SKILL AIRTIME ####
                PlayerManager.Instance().Skill_AirTime();
                //#######################

                timescale = slowDownTimescale;
                Vibration.Vibrate(3);
                playerRenderer.material.color = Color.yellow;
                //playerRenderer.material = dicPlayerMats["PlayerAdditiveMat"];
                glintAnimation.SetTrigger("Trigger");

                airJumpLight.StartBlink();
            }
        }
        else
        {
            if (isFainting && !isFalling && !startDeathForUpgradeAct)
            {
                // Fainting on the ground
                isFainting = false;
            }
        }
    }

    bool CheckGround()
    {
        if (Mathf.Approximately(playerRigidbody2D.velocity.y, 0)
        && IsRaycastHitGround())
        {
            currentRemainJump = possibleMaxJump;
            isAirJump = false;
            playerRenderer.material.color = Color.white;

            isHurting = false;
            //playerRenderer.material = dicPlayerMats["PlayerMat"];
            timescale = 1.0f;
            airJumpLight.Hide();
            return true;
        }

        return false;
    }

    bool IsRaycastHitGround()
    {
        float groundCollideXSize = Mathf.Abs(groundEdgeCollider.points[1].x - groundEdgeCollider.points[0].x);
        Vector2 groundCheckColliderSize = new Vector2(groundCollideXSize, 0.1f);
        Vector3 castOrigin = groundCheckCenter.position;
        castOrigin.y -= 0.21f;
        RaycastHit2D groundHit = Physics2D.BoxCast(castOrigin, groundCheckColliderSize, 0f, Vector2.down, 0f);

        if (groundHit) groundCollideObject = groundHit.collider;
        else groundCollideObject = null;

        return groundHit;
    }

    void UpdateAnimator()
    {
        playerAnimator.SetBool("isGround", isGround);
        playerAnimator.SetBool("isHoldingTouch", flickController.GetIsHolding());
        playerAnimator.SetFloat("vSpeed", playerRigidbody2D.velocity.y);
        playerAnimator.SetBool("isAirJump", isAirJump);
        playerAnimator.SetBool("isHurting", isHurting);
        playerAnimator.SetBool("isDead", PlayerManager.Instance().IsDead);
        playerAnimator.SetBool("isFalling", isFalling);
        playerAnimator.SetBool("isFainting", isFainting);
        playerAnimator.SetBool("startDeathForUpgradeAct", startDeathForUpgradeAct);

        glintAnimation.speed = 1f / Time.timeScale;

        //Debug.Log("vSpeed : " + playerRigidbody2D.velocity.y);

        // Check x-Flip
        Vector3 tVec = playerAnimator.transform.localScale;
        if ((tVec.x < 0 && playerRigidbody2D.velocity.x > 0)
        || (tVec.x > 0 && playerRigidbody2D.velocity.x < 0))
        {
            
            if (isHurting == true) 
            {
                if (playerRigidbody2D.velocity.x > 0) tVec.x = -Mathf.Abs(tVec.x);
                else tVec.x = Mathf.Abs(tVec.x);
            }
            else tVec.x *= -1;

            playerAnimator.transform.localScale = tVec;
        }

        if (flickController.GetIsHolding() && isGround)
        {
            if (flickController.GetFlickedVector().x < 0) tVec.x = characterScale;
            else if (flickController.GetFlickedVector().x > 0) tVec.x = -characterScale;

            playerAnimator.transform.localScale = tVec;
        }

        // DoubleJump Cooltime
        if (PlayerManager.Instance().GetIsAirTime())
        {
            skillCooltime.SetActive(true);
            Vector3 progressVector = skillCooltime.transform.localScale;
            double scaleX = 1f - (PlayerManager.Instance().GetAirTimeTimer() / PlayerManager.Instance().PlayerStatus.AirTimeDuration);
            progressVector.x = (float)scaleX * 4f;
            skillCooltime.transform.localScale = progressVector;
        }
        else
        {
            skillCooltime.SetActive(false);
        }

        // DamageText Effect
        Color t = textDamagedObject.color;
        t.a = textDamagedAlpha.value;
        textDamagedObject.color = t;
    }   

    void ControlGroundCollide()
    {
        if (groundCollideObject != null)
        {
            if ((groundCollideObject.tag == "MovingPlatform") && isGround)
            {
                this.transform.parent = groundCollideObject.transform;
            }
            else this.transform.parent = PlayerManager.Instance().transform;
        }
        else
        {
            if (this.transform.parent != PlayerManager.Instance().transform)
            {
                this.transform.parent = PlayerManager.Instance().transform;
            }
        }
    }

    public GameObject GetJumpGuide()
    {
        return jumpGuide;
    }


    void OnTriggerStay2D(Collider2D collider)
    {
        if (PlayerManager.Instance().IsDead) return;

        if (collider != null)
        {
            switch(collider.tag)
            {
                case "Damage_spike":{
                    if (canTriggerHurt && !reviveTime)
                    {
                        //Debug.Log("Damage_spike");
                        //Debug.Log("---------------------Hit!");
                        damagedLight.StartBlink(this.unbeatableDuration_hurt, Color.red);
                        StartCoroutine(TriggerHurt(collider, this.unbeatableDuration_hurt));
                        PlayerManager.Instance().OnDamaged(DAMAGED_TYPE.SPIKE);                        
                    }
                }
                break;
                case "MoveTrigger":{
                        playerRigidbody2D.velocity = Vector2.zero;
                        MoveTrigger trigger = collider.GetComponent<MoveTrigger>();

                        if (trigger != null)
                        {
                            if (trigger.TriggerOn)
                            {
                                if (trigger.moveTrigger == MOVE_TRIGGER.MOVE_POSITION)
                                {
                                    Transform tempTarget = collider.GetComponent<MoveTrigger>().targetPosition;
                                    playerRigidbody2D.position = new Vector2(tempTarget.position.x, tempTarget.position.y);
                                    //playerRigidbody2D.MovePosition(new Vector2(tempTarget.position.x, tempTarget.position.y));
                                }
                                else if (trigger.moveTrigger == MOVE_TRIGGER.MOVE_NEXTSTAGE)
                                {
                                    TopMostControl.Instance().StartChangeScene(SCENE_INDEX.GAMESTAGE, true, StageLoader.CurrentStage + 1);
                                    trigger.TriggerOn = false;
                                }
                            }
                        }
                }
                break;
            }       
        }
    }

    public void ShowDamageText(int damageNumber)
    {
        textDamagedObject.gameObject.SetActive(true);
        textDamagedObject.text = "-"+damageNumber.ToString();
        textDamagedAlpha.Begin();
        textDamagedTransform.Begin();

        textDamagedAlpha.value = textDamagedAlpha.startValue;
        textDamagedTransform.vector3Results = textDamagedTransform.startingVector;

        textDamagedAlpha.TweenCompleted += HideDamageText;
    }

    void HideDamageText()
    {
        textDamagedObject.text = string.Empty;
        textDamagedObject.gameObject.SetActive(false);
        textDamagedAlpha.TweenCompleted -= HideDamageText;
    }

    

    IEnumerator TriggerHurt(Collider2D collider, float unbeatableDuration)
    {
        float timer = 0f;
        canTriggerHurt = false;
        isHurting = true;
        currentRemainJump = 0;
        timescale = 1.0f;

        // Controll Rigidbody
        bool isLeftKnockback 
        = (collider.transform.position.x > playerRigidbody2D.transform.position.x) ? true : false;

        float xKnockbackPower = Mathf.Abs(knockbackXPower);
        if (isLeftKnockback) xKnockbackPower = -xKnockbackPower;

        playerRigidbody2D.velocity = Vector2.zero;
        playerRigidbody2D.AddForce(new Vector2(xKnockbackPower, knockbackYPower), ForceMode2D.Impulse);

        while (timer < unbeatableDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        canTriggerHurt = true;

        yield break;
    }

    IEnumerator TriggerOverWhelming(float unbeatableDuration)
    {
        reviveLight.StartBlink(unbeatableDuration, Color.yellow);
        float timer = 0f;
        reviveTime = true;
        isHurting = false;
        timescale = 1.0f;

        playerRigidbody2D.velocity = Vector2.zero;

        while (timer < unbeatableDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        reviveTime = false;

        yield break;
    }

    void OnFinishAirTime()
    {
        timescale = 1.0f;
        if (currentRemainJump > 0) currentRemainJump -= 1;
        airJumpLight.StartBlink(0.7f, ColorBlink_AirJump);
    }

    void OnDeath(bool isDead)
    {
        if (isDead == false)
        {
            // Revive!
            isFainting = false;
            StartCoroutine(TriggerOverWhelming(this.unbeatableDuration_revive));
        }
    }

    public Rigidbody2D GetPlayerRigidBody()
    {
        return playerRigidbody2D;
    }

    void OnClickReturnButton()
    {
        // Death for Upgrade
        if (isGround && !PlayerManager.Instance().IsDead && !startDeathForUpgradeAct)
        {
            startDeathForUpgradeAct = true;
            PlayerManager.Instance().PlayerStatus.CurrentHP = 0;
            damagedLight.StartBlink(2f, Color.red);

            TopMostControl.Instance().GameOver(true);
            TopMostControl.Instance().StartGlobalLightEffect(Color.magenta, 3f, 0.3f);

            if (PlayerManager.Instance().CameraController() != null)
            {
                PlayerManager.Instance().CameraController().CameraShake_Rot(5);
            }
        }
    }

    void OnClickGameOverMenu(MENU_GAMEOVER menu)
    {
        switch (menu)
        {
            case MENU_GAMEOVER.MAINMENU:
                break;
            case MENU_GAMEOVER.RETRY:
                if (startDeathForUpgradeAct)
                {
                    startDeathForUpgradeAct = false;
                }
                break;
            case MENU_GAMEOVER.REVIVE_SHARDS:
                break;
            case MENU_GAMEOVER.REVIVE_AD:
                break;
            default:
                break;
        }
    }

    public void BlinkPlayerShardLight(Color shardColor)
    {
        shardLight.StartBlink(0.5f, shardColor);
    }


}
