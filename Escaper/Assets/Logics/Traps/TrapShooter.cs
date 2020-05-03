using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapShooter : MonoBehaviour
{
    [System.Serializable]
    public struct pattern{
        public float delay;
        public float speed;
    }

    public SpriteRenderer bodySprite;

    public GameObject projectile;
    public float projectileSpeed;
    public float activateOnDistance = 700f;

    public pattern[] shootDelayPattern;

    public bool activatingTrap = false;

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
                    if (++currentPatternIndex >= shootDelayPattern.Length) currentPatternIndex = 0;
                }
            }
        }
    }


    void CheckActivateTrap()
    {
        if (Vector3.Distance(this.transform.position, PlayerManager.Instance().playerController.GetPlayerRigidBody().transform.position)
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

    void SpriteColorWarning(float currentTimer, float delayTime)
    {
        float ratio = 1 - (currentTimer / delayTime);

        Color warnColor = Color.red;
        warnColor.g = ratio;
        warnColor.b = ratio;

        bodySprite.color = warnColor;
    }
}
