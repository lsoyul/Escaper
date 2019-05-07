using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float accelerate = 0.1f;
    public float friction = 0.04f;

    public float maxSpeed = 1.0f;

    private Vector3 currentVelocity;
    private Vector3 currentAcceleration;

    private Rigidbody playerRigidBody;

    private Vector3 currentPosition;

    private void Start()
    {
        currentPosition = this.transform.position;

        playerRigidBody = GetComponent<Rigidbody>();

        InitializeVariables();
    }

    public void Move(Vector3 controllerVelocity)
    {
        currentVelocity += controllerVelocity * accelerate;

        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity = currentVelocity.normalized * maxSpeed;
        }

        
    }

    private void FixedUpdate()
    {
        playerRigidBody.MovePosition(playerRigidBody.position + currentVelocity * Time.fixedDeltaTime);
        
        if (currentVelocity.magnitude >= friction)
        {
            currentVelocity -= (currentVelocity.normalized * friction);
        }
        else
        {
            currentVelocity = Vector3.zero;
            playerRigidBody.velocity = Vector3.zero;
        }
    }

    public void InitializeVariables()
    {
        currentVelocity = Vector3.zero;
        currentAcceleration = Vector3.zero;
    }
}
