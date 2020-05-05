using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SoundManagerNamespace;

public class TrapShooter : MonoBehaviour
{
    [System.Serializable]
    public struct pattern{
        public float delay;
        public float speed;
    }

    public SpriteRenderer bodySprite;

    public GameObject projectile;
    public float activateOnDistance = 700f;

    public pattern[] shootDelayPattern;

    public bool activatingTrap = false;

    [Header("- Following ? -")]
    public bool IsFollowPlayer = false;

    private float currentTimer;
    private int currentPatternIndex = 0;

    private void Start()
    {
        projectile.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckActivateTrap();

        if (activatingTrap == true)
        {
            if (currentPatternIndex < shootDelayPattern.Length)
            {
                currentTimer += Time.deltaTime;

                // Sprite Color Warning
                SpriteColorWarning(currentTimer, shootDelayPattern[currentPatternIndex].delay);

                if (currentTimer >= shootDelayPattern[currentPatternIndex].delay)
                {
                    // Shoot!
                    GameObject go = Instantiate(projectile.gameObject, this.transform.position + (this.transform.up * 10), this.transform.localRotation) as GameObject;
                    go.SetActive(true);
                    DirectionalProjectile proj = go.GetComponent<DirectionalProjectile>();
                    proj.Fire(shootDelayPattern[currentPatternIndex].speed, GameStatics.PROJECTILE_TYPE.SHOOTER1);

                    currentTimer = 0;

                    if (IsFollowPlayer == true)
                        SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_trapFire1], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_trapFire1].clip);
                    
                    if (++currentPatternIndex >= shootDelayPattern.Length) currentPatternIndex = 0;
                }
            }

            if (IsFollowPlayer == true)
            {
                if (PlayerManager.Instance().GetPlayerControl() != null)
                {
                    Vector3 targetPos = PlayerManager.Instance().GetPlayerControl().GetPlayerRigidBody().transform.position - (Vector3.up * 3);
                    Vector3 followDirection = (targetPos - this.transform.position).normalized;

                    this.transform.up = followDirection;
                }
            }
        }
    }


    void CheckActivateTrap()
    {
        if (PlayerManager.HasInstance())
        {
            if (Vector3.Distance(this.transform.position, PlayerManager.Instance().GetPlayerControl().GetPlayerRigidBody().transform.position)
                 < activateOnDistance)
            {
                // Activate Trap
                activatingTrap = true;
            }
            else
            {
                activatingTrap = false;
                currentTimer = 0;
                currentPatternIndex = 0;
            }
        }
        else
        {
            activatingTrap = true;
        }
    }

    void SpriteColorWarning(float currentTimer, float delayTime)
    {
        float ratio = 1 - (currentTimer / delayTime);

        Color warnColor = Color.red;
        warnColor.g = ratio;
        warnColor.b = ratio;

        bodySprite.color = warnColor;
    }
}
