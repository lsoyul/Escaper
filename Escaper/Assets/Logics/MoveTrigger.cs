using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrigger : MonoBehaviour
{
    public GameStatics.MOVE_TRIGGER moveTrigger;
    public Transform targetPosition;

    public bool TriggerOn = false;

    private void OnEnable()
    {
        TriggerOn = true;
    }
}
