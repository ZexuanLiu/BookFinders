using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class JoystickLook : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerExitHandler, IEndDragHandler
{
    [SerializeField] GameObject userCamera;
    [SerializeField] Image joystickImage;

    private ICameraMovement cameraWithMovementInterface;

    private static float initialRadiousOfJoystick = 100;

    private float joystickRadiusScaled;
    private float joystickStartingX;
    private float joystickStartingY;

    public void OnDrag(PointerEventData eventData)
    {
        CheckAndUpdateJoystickProperties();
        UpdateCameraPosition(eventData.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CheckAndUpdateJoystickProperties();
        UpdateCameraPosition(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdateCameraPosition(new Vector2(joystickStartingX, joystickStartingY));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UpdateCameraPosition(new Vector2(joystickStartingX, joystickStartingY));
    }

    private void CheckAndUpdateJoystickProperties()
    {
        joystickRadiusScaled = joystickImage.rectTransform.lossyScale.x * initialRadiousOfJoystick;
        joystickStartingX = gameObject.transform.position.x;
        joystickStartingY = gameObject.transform.position.y;
    }

    public void UpdateCameraPosition(Vector2 pressPositionRaw)
    {
        float trueX = pressPositionRaw.x - joystickStartingX;
        float trueY = pressPositionRaw.y - joystickStartingY;

        if (trueX > joystickRadiusScaled)
        {
            trueX = joystickRadiusScaled;
        }
        else if (trueX < -1 * joystickRadiusScaled)
        {
            trueX = -1 * joystickRadiusScaled;
        }
        if (trueY > joystickRadiusScaled)
        {
            trueY = joystickRadiusScaled;
        }
        else if (trueY < -1 * joystickRadiusScaled)
        {
            trueY = -1 * joystickRadiusScaled;
        }

        float trueXNormalized = trueX / joystickRadiusScaled;
        float trueYNormalized = trueY / joystickRadiusScaled;

        cameraWithMovementInterface.UpdateCameraRotation(trueXNormalized, trueYNormalized);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (userCamera.TryGetComponent(out ICameraMovement cameraMovement))
        {
            cameraWithMovementInterface = cameraMovement;
        }
        else
        {
            throw new Exception("Camera has no ICameraMovement");
        }

        CheckAndUpdateJoystickProperties();
    }
}
