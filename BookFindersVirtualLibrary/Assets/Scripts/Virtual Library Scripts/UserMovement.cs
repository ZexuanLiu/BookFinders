using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6;
    [SerializeField] float jumpSpeed = 6;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;

    float horizontalInput;
    float verticalInput;

    [SerializeField] Transform orientation;

    Vector3 movementDirection;
    Rigidbody thisBody;

    [SerializeField] float groundDrag;
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ground;
    bool grounded;
    bool readyJump = true;
    
    // Start is called before the first frame update
    void Start()
    {
        thisBody = GetComponent<Rigidbody>();
        thisBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, (playerHeight * 0.5f) + 2f, ground);
        if (grounded)
        {
            thisBody.drag = groundDrag;
        }
        else
        {
            thisBody.drag = 0;
        }

        Inputs();
        SpeedControl();

        MovePlayer();
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void ResetJump()
    {
        readyJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(thisBody.velocity.x, 0f, thisBody.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            thisBody.velocity = new Vector3(limitedVel.x, thisBody.velocity.y, limitedVel.z);
        }
    }


    private void MovePlayer()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            thisBody.AddForce(movementDirection.normalized * moveSpeed, ForceMode.Force);
        }
        else
        {
            thisBody.AddForce(movementDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }

        bool jumpPressed = Input.GetButtonDown("Jump");
        if (jumpPressed && readyJump && grounded)
        {
            readyJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    void Jump()
    {
        thisBody.velocity = new Vector3(thisBody.velocity.x, 0f, thisBody.velocity.z);
        thisBody.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
    }

}
