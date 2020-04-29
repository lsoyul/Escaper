using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
    public GameStatics.MOVE_TRIGGER moveTrigger;
    public GameStatics.PORTAL_TYPE portalType;

    public Transform targetPosition;

    public GameObject FromPortalOff;
    public GameObject FromPortalOn;
    public GameObject ToPortalOff;
    public GameObject ToPortalOn;

    [Header("- Portal Effect -")]
    public GameObject FromPortalEffect;
    public GameObject ToPortalEffect;

    public bool TriggerOn = false;

    public void SetPortal(bool isOn)
    {
        if (moveTrigger == GameStatics.MOVE_TRIGGER.MOVE_POSITION)
        {

            FromPortalOff.SetActive(!isOn);
            FromPortalOn.SetActive(isOn);
            ToPortalOff.SetActive(!isOn);
            ToPortalOn.SetActive(isOn);

            FromPortalEffect.SetActive(isOn);
            ToPortalEffect.SetActive(isOn);
        }

        TriggerOn = isOn;
    }

    public void OnEnable()
    {
        if (moveTrigger == GameStatics.MOVE_TRIGGER.MOVE_NEXTSTAGE)
        {
            FromPortalOff.SetActive(false);
            FromPortalOn.SetActive(false);
            ToPortalOff.SetActive(false);
            ToPortalOn.SetActive(false);

            FromPortalEffect.SetActive(false);
            ToPortalEffect.SetActive(false);

            TriggerOn = true;
        }
        else if (moveTrigger == GameStatics.MOVE_TRIGGER.MOVE_POSITION)
        {
            bool isOnPortal = GameConfigs.PortalStatus(portalType);

            SetPortal(isOnPortal);
        }
    }

    public void Update()
    {
        if (TriggerOn == false && moveTrigger == GameStatics.MOVE_TRIGGER.MOVE_POSITION)
        {
            //Check portal purchase collision

            if (Vector3.Distance(PlayerManager.Instance().GetPlayerControl().GetPlayerRigidBody().transform.position, ToPortalOff.transform.position)
                < 20f)
            {
                TopMostControl.Instance().IsCollidingPortalPurchase = true;

                if (TopMostControl.Instance().IsShowingPortalPurchaseUI == false)
                {
                    TopMostControl.Instance().ShowPortalPurchaseUI(true, this);
                }
            }
            else
            {
                TopMostControl.Instance().IsCollidingPortalPurchase = false;
            }
        }
        else
        {
            TopMostControl.Instance().IsCollidingPortalPurchase = false;
        }
    }
}
