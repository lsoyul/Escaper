using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DigitalRuby.SoundManagerNamespace;
using UnityEngine.Experimental.Rendering.Universal;
using System.Security.Cryptography;

public class TrapEarthQuake : MonoBehaviour
{
    [System.Serializable]
    public struct pattern
    {
        public float delay;
    }

    public bool detectActivate = true;
    public Transform triggerMinYAxis;
    public Transform triggerMaxYAxis;

    public Transform damageAreaMinY;
    public Transform damageAreaMaxY;


    public List<Light2D> warnLights;
    public float maxIntensity = 5;

    public pattern[] quakePatterns;
    private int currentPatternIndex = 0;
    private float currentUnscaledTimer = 0;

    private void Start()
    {
        if (warnLights != null)
        {
            foreach (Light2D light in warnLights)
            {
                light.intensity = 0;
            }
        }

    }

    private void Update()
    {
        if (CheckActivateTrap() == true)
        {
            if (currentPatternIndex < quakePatterns.Length)
            {
                currentUnscaledTimer += Time.unscaledDeltaTime;

                SetLightIntensity(currentUnscaledTimer, quakePatterns[currentPatternIndex].delay);

                if (currentUnscaledTimer >= quakePatterns[currentPatternIndex].delay)
                {
                    // Quake!
                    if (PlayerManager.Instance().playerController.IsGround() &&
                        PlayerManager.Instance().playerController.GetPlayerRigidBody().transform.position.y > damageAreaMinY.position.y &&
                        PlayerManager.Instance().playerController.GetPlayerRigidBody().transform.position.y < damageAreaMaxY.position.y)
                    {

                        if (!PlayerManager.Instance().IsDead &&
                            PlayerManager.Instance().playerController.canTriggerHurt &&
                            !PlayerManager.Instance().playerController.reviveTime)
                        {
                            StartCoroutine(PlayerManager.Instance().playerController.TriggerHurt(null, 
                                PlayerManager.Instance().playerController.unbeatableDuration_hurt));

                            PlayerManager.Instance().playerController.isFainting = true;

                            PlayerManager.Instance().OnDamaged(GameStatics.DAMAGED_TYPE.EARTH_QUAKE);
                        }
                    }

                    SoundManager.PlayOneShotSound(SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_trapFire1], SoundContainer.Instance().SoundEffectsDic[GameStatics.sound_trapFire1].clip);
                    PlayerManager.Instance().CameraController().CameraShake_Rot(1);

                    currentUnscaledTimer = 0;
                    if (++currentPatternIndex >= quakePatterns.Length) currentPatternIndex = 0;
                }
            }
        }
    }


    void SetLightIntensity(float currentTime, float targetDelayTime)
    {
        float ratio = currentTime / targetDelayTime;

        foreach (Light2D light in warnLights)
        {
            light.intensity = maxIntensity * ratio;
        }
    }

    bool CheckActivateTrap()
    {
        if (detectActivate && PlayerManager.HasInstance())
        {
            if (PlayerManager.Instance().playerController.GetPlayerRigidBody().transform.position.y > triggerMinYAxis.position.y
                && PlayerManager.Instance().playerController.GetPlayerRigidBody().transform.position.y < triggerMaxYAxis.position.y)
            {
                return true;
            }
            else
            {
                currentUnscaledTimer = 0;
                currentPatternIndex = 0;

                SetLightIntensity(currentUnscaledTimer, quakePatterns[currentPatternIndex].delay);
            }
        }

        return false;
    }
}
