using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


interface IMovementControl
{
    public void ApplyMovement(float x, float y);

    public void ApplyJump();

    public bool IsUsingControllerInput();
}

public class UserMovement : MonoBehaviour, IMovementControl
{
    [SerializeField] float moveSpeed = 12;
    [SerializeField] float jumpSpeed = 12;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;

    private float horizontalInput;
    private float verticalInput;

    [SerializeField] GameObject flashText;
    private IFlashable iFlashable;

    [SerializeField] Transform orientation;

    private Vector3 movementDirection;
    private Rigidbody thisBody;

    [SerializeField] float groundDrag;
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ground;
    private bool grounded;
    private bool readyJump = true;

    private bool isUsingControllerInput = true;
    
    // Start is called before the first frame update
    void Start()
    {
        //#if UNITY_EDITOR
        //    QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //    Application.targetFrameRate = 45;
        //#endif

        thisBody = GetComponent<Rigidbody>();
        thisBody.freezeRotation = true;

        if (flashText.TryGetComponent(out IFlashable flashable))
        {
            iFlashable = flashable;
        }
        else
        {
            throw new Exception("User has no IFlashable");
        }
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, (playerHeight * 0.5f) + 1.5f, ground);
        if (grounded)
        {
            thisBody.drag = groundDrag;
        }
        else
        {
            thisBody.drag = 0;
        }

        if (Input.GetKeyDown(KeyCode.Tab)){
            isUsingControllerInput = !isUsingControllerInput;
            horizontalInput = 0;
            verticalInput = 0;
        }

        Inputs();
        SpeedControl();

        MovePlayer();
    }



    private void Inputs()
    {
        if (!isUsingControllerInput)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
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
        //iFlashable.Flash($"{movementDirection}");
        //iFlashable.Flash($"{orientation.forward * verticalInput} + {orientation.right * horizontalInput}");

        Vector3 forceAdded = Vector3.zero;
        if (grounded)
        {
            forceAdded = movementDirection * moveSpeed * Time.deltaTime * 350;
        }
        else
        {
            forceAdded = movementDirection * moveSpeed * airMultiplier * Time.deltaTime * 350;
        }
        //iFlashable.Flash($"{forceAdded}");

        thisBody.AddForce(forceAdded, ForceMode.Force);

        bool jumpPressed = Input.GetButtonDown("Jump");
        if (jumpPressed && !isUsingControllerInput)
        {
            ApplyJump();
        }

    }

    void Jump()
    {
        thisBody.velocity = new Vector3(thisBody.velocity.x, 0f, thisBody.velocity.z);
        thisBody.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
    }

    public void ApplyMovement(float x, float y)
    {
        horizontalInput = x;
        verticalInput = y;
    }

    public void ApplyJump()
    {
        if (readyJump && grounded)
        {
            readyJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public bool IsUsingControllerInput()
    {
        return isUsingControllerInput;
    }
}
