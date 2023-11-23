using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

interface ICameraMovement
{
    public void UpdateCameraRotation(float x, float y);
}

public class UserCamera : MonoBehaviour, ICameraMovement
{
    [SerializeField] float sensX;
    [SerializeField] float sensY;
    [SerializeField] bool useMouseLook = true;

    [SerializeField] Transform orientation;

    [SerializeField] GameObject userPathingObject;
    [SerializeField] GameObject arrowInFront;
    private IFindingPathTo findingPath;

    private float mouseXRaw;
    private float mouseYRaw;

    private float xRotation;
    private float yRotation;

    void Start()
    {
        UpdateCursorMode();

        if (userPathingObject.TryGetComponent(out IFindingPathTo findingPathInterface))
        {
            findingPath = findingPathInterface;
        }
        else
        {
            throw new Exception("UserPathing has no IFindingPathTo");
        }
    }

    private void UpdateMouseInputs()
    {
        mouseXRaw = Input.GetAxisRaw("Mouse X");
        mouseYRaw = Input.GetAxisRaw("Mouse Y");
    }

    // Update is called once per frame
    void Update()
    {

        float cameraXRotation = 0;
        float cameraYRotation = 0;
        if (useMouseLook)
        {
            UpdateMouseInputs();
            cameraXRotation = mouseXRaw * Time.deltaTime * sensX;
            cameraYRotation = mouseYRaw * Time.deltaTime * sensY;
        }
        else
        {
            cameraXRotation = mouseXRaw * Time.deltaTime * sensX/10;
            cameraYRotation = mouseYRaw * Time.deltaTime * sensY/10;
        }

        yRotation += cameraXRotation;
        xRotation -= cameraYRotation;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Debug.Log($"{xRotation}. {yRotation}");

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        Vector3 arrowDestination = findingPath.ArrowDestination();
        if (arrowDestination == Vector3.zero)
        {
            arrowInFront.SetActive(false);
        }
        else
        {
            arrowInFront.SetActive(true);
            arrowInFront.transform.LookAt(arrowDestination);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            useMouseLook = !useMouseLook;
            UpdateCursorMode();
        }
    }

    public void UpdateCameraRotation(float x, float y)
    {
        mouseXRaw = x;
        mouseYRaw = y;
    }

    public void UpdateCursorMode()
    {
        if (useMouseLook)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}


