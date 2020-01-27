using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public bool isLoop = true;
    public bool moveFinish = false;
    public float moveSpeed = 0.1f;
    public Vector3 fromPosition;
    public Vector3 toPosition;
    public bool moveForward = true;
    private Vector3 currentTargetPos;
    private Vector3 forwardUnitVector;
    private Vector3 limitLeftPosition;
    private Vector3 limitRightPosition;
    private float initMoveSpeed = 0f;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        currentTargetPos = new Vector3();
        limitLeftPosition = new Vector3();
        limitRightPosition = new Vector3();
        currentTargetPos = toPosition;
        moveForward = true;
        initMoveSpeed = moveSpeed;

        forwardUnitVector = toPosition - fromPosition;
        forwardUnitVector.Normalize();

        if (toPosition.x > fromPosition.x){
            limitLeftPosition = this.transform.position + fromPosition;
            limitRightPosition = this.transform.position + toPosition;
        }
        else{
            limitLeftPosition = this.transform.position + toPosition;
            limitRightPosition = this.transform.position + fromPosition;
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (Time.timeScale < 1.0f) moveSpeed = initMoveSpeed * GameStatics.fixedDeltaOffset;
        else moveSpeed = initMoveSpeed;


        if (moveFinish == false){
            Vector3 nextPos = this.transform.position;

            if (moveForward == true){
                nextPos += forwardUnitVector * moveSpeed;
            }
            else{
                nextPos -= forwardUnitVector * moveSpeed;
            }

            if (nextPos.x < limitLeftPosition.x)
            {
                nextPos = limitLeftPosition;
                if (isLoop) moveForward = !moveForward;
                else moveFinish = true;
            }
            else if (nextPos.x > limitRightPosition.x)
            {
                nextPos = limitRightPosition;
                if (isLoop) moveForward = !moveForward;
                else moveFinish = true;
            }

            this.transform.position = nextPos;
        }
    }


}
