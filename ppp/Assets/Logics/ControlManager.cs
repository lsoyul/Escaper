using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public Player targetPlayer;
    public FloatingJoystick floatingJoystick;

    public void FixedUpdate()
    {
        Vector3 velocity = Vector3.forward * floatingJoystick.Vertical + Vector3.right * floatingJoystick.Horizontal;

        targetPlayer.Move(velocity);
    }

}
