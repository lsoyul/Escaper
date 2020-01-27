using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickController : MonoBehaviour
{
    public FloatingJoystick floatingJoystick;
    private Vector2 direction;
    public float multipliedValue = 5f;
    private float flickPower = 0f;

    bool isHolding = false;

    public System.Action onPointerDown;
    public System.Action onPointerUp;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake(){
        floatingJoystick.onPointerUp += OnPointerUp;
        floatingJoystick.onPointerDown += OnPointerDown;
    }

    private void OnDestroy() {
        floatingJoystick.onPointerUp -= OnPointerUp;
        floatingJoystick.onPointerDown -= OnPointerDown;    
    }

    public void FixedUpdate()
    {
        direction = Vector2.up * floatingJoystick.Vertical + Vector2.right * floatingJoystick.Horizontal;
        flickPower = direction.magnitude * multipliedValue;
    }

    public Vector2 GetFlickedVector()
    {
        return direction.normalized * flickPower;
    }

    void OnPointerUp()
    {
        isHolding = false;
        if (onPointerUp != null) onPointerUp();
    }

    void OnPointerDown()
    {
        isHolding = true;
        if (onPointerDown != null) onPointerDown();
    }

    public bool GetIsHolding()
    {
        return isHolding;
    }
}

