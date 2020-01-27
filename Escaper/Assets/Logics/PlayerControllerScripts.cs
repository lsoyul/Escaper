using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScripts : MonoBehaviour
{
    public FlickController flickController;
    public GameObject jumpGuide;
    public Rigidbody2D playerRigidbody2D;
    public EdgeCollider2D groundEdgeCollider;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;
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

    // Ground Check
    [SerializeField] bool isGround = false;
    [SerializeField] bool isAirJump = false;

    [Header("- Sprite Renderer -")]
    public SpriteRenderer playerRenderer;
    public Material[] playerMats;
    private Dictionary<string, Material> dicPlayerMats;
    private float initFixedDeltaTime = 0f;
    [SerializeField] private float slowDownTimescale = 0.1f;

    [SerializeField] private Collider2D groundCollideObject = null;

    private void Awake() {
        if (flickController == null)
        {
            Debug.LogError("FlickController is null");
        }

        characterScale = playerAnimator.transform.localScale.x;
        dicPlayerMats = new Dictionary<string, Material>();
        
        foreach( Material mat in playerMats)
        {
            dicPlayerMats.Add(mat.name, mat);
        }

        flickController.onPointerUp += OnPointerUp;
        flickController.onPointerDown += OnPointerDown;

        initFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void OnDestroy() {
        if (flickController != null)
        {
            flickController.onPointerUp -= OnPointerUp;
            flickController.onPointerDown -= OnPointerDown;
        }
    }

    private void Update() {
        Time.timeScale = timescale;
        if (timescale < 1.0f) Time.fixedDeltaTime = slowDownTimescale * timescale * GameStatics.fixedDeltaOffset;
        else Time.fixedDeltaTime = initFixedDeltaTime;
        
        isGround = CheckGround();

        if (flickController.GetIsHolding() && currentRemainJump > 0)
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

            //if (isGround == false) Eff_jumpCharge.SetActive(true);
        }
        else
        {
            jumpGuide.SetActive(false);
            //Eff_jumpCharge.SetActive(false);
        }

        if (playerRigidbody2D.velocity.y < minimumYSpeed)
        {
            Vector2 t = playerRigidbody2D.velocity;
            t.y = minimumYSpeed;
            playerRigidbody2D.velocity = t;
        }

        ControlGroundCollide();

        UpdateAnimator();
    }



    void OnPointerUp()
    {
        if (flickController.GetFlickedVector().magnitude > 1f && currentRemainJump > 0)
        {
            playerRigidbody2D.velocity = Vector2.zero;
            playerRigidbody2D.AddForce(-flickController.GetFlickedVector(), ForceMode2D.Impulse);

            if (isGround == false) 
            {
                isAirJump = true;
                Vibration.Vibrate(1);
                //GameObject go = Instantiate(Eff_jumpCloud, Eff_jumpCloud.transform.position, Eff_jumpCloud.transform.rotation);
                //ParticleAutoDestroy particleAutoDestroy = go.GetComponent<ParticleAutoDestroy>();
                //particleAutoDestroy.StartAutoDestroy();
            }

        }

        currentRemainJump -= 1;
        timescale = 1.0f;
        playerRenderer.material.color = Color.white;
        //playerRenderer.material = dicPlayerMats["PlayerMat"];
    }

    void OnPointerDown()
    {
        if (currentRemainJump > 0 && isGround == false)
        {
            timescale = slowDownTimescale;
            Vibration.Vibrate(3);
            playerRenderer.material.color = Color.yellow;
            //playerRenderer.material = dicPlayerMats["PlayerAdditiveMat"];
            glintAnimation.SetTrigger("Trigger");
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
            //playerRenderer.material = dicPlayerMats["PlayerMat"];
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

        //RaycastHit2D leftHit = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, 0.1f);
        //RaycastHit2D rightHit = Physics2D.Raycast(groundCheckRight.position, Vector2.down, 0.1f);

        //Debug.DrawRay(groundCheckLeft.position, Vector2.down, Color.red);
        //Debug.DrawRay(groundCheckRight.position, Vector2.down, Color.red);

        //if (leftHit) Debug.Log("LeftHit :" + leftHit);
        //if (rightHit) Debug.Log("Right Hit:" + rightHit);

        //return leftHit || rightHit;
    }

    void UpdateAnimator()
    {
        playerAnimator.SetBool("isGround", isGround);
        playerAnimator.SetBool("isHoldingTouch", flickController.GetIsHolding());
        playerAnimator.SetFloat("vSpeed", playerRigidbody2D.velocity.y);
        playerAnimator.SetBool("isAirJump", isAirJump);
        glintAnimation.speed = 1f / Time.timeScale;

        //Debug.Log("vSpeed : " + playerRigidbody2D.velocity.y);

        // Check x-Flip
        Vector3 tVec = playerAnimator.transform.localScale;
        if ((tVec.x < 0 && playerRigidbody2D.velocity.x > 0)
        || (tVec.x > 0 && playerRigidbody2D.velocity.x < 0))
        {
            tVec.x *= -1;
            playerAnimator.transform.localScale = tVec;
        }

        if (flickController.GetIsHolding() && isGround)
        {
            if (flickController.GetFlickedVector().x < 0) tVec.x = characterScale;
            else if (flickController.GetFlickedVector().x > 0) tVec.x = -characterScale;

            playerAnimator.transform.localScale = tVec;
        }
    }   

    void ControlGroundCollide()
    {
        if (groundCollideObject != null)
        {
            if ((groundCollideObject.tag == "MovingPlatform") && isGround)
            {
                this.transform.parent = groundCollideObject.transform;
            }
            else this.transform.parent = null;
        }
        else
        {
            if (this.transform.parent != null) this.transform.parent = null;
        }
    }

    public GameObject GetJumpGuide()
    {
        return jumpGuide;
    } 

}
