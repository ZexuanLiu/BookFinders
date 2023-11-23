using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class JoystickMovement : MonoBehaviour, IDragHandler, IPointerClickHandler, IPointerExitHandler, IEndDragHandler
{
    [SerializeField] GameObject userWithMovement;
    [SerializeField] Image joystickImage;


    private IMovementControl userWithMovementInterface;

    [SerializeField] GameObject flashText;
    private IFlashable iFlashable;

    private static float initialRadiousOfJoystick = 100;

    private float joystickRadiusScaled;
    private float joystickStartingX;
    private float joystickStartingY;

    public void OnDrag(PointerEventData eventData)
    {
        userWithMovementInterface.UsingIMovement(true);
        CheckAndUpdateJoystickProperties();
        UpdatePlayerPosition(eventData.position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        userWithMovementInterface.UsingIMovement(true);
        CheckAndUpdateJoystickProperties();
        UpdatePlayerPosition(eventData.position);

        //GameObject joystickObject = eventData.pointerPress;
        //Vector3 center = joystickObject.transform.position;
        //iFlashable.Flash($"{center.x},{center.y}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdatePlayerPosition(new Vector2(joystickStartingX, joystickStartingY));
        userWithMovementInterface.UsingIMovement(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UpdatePlayerPosition(new Vector2(joystickStartingX, joystickStartingY));
        userWithMovementInterface.UsingIMovement(false);
    }

    private void CheckAndUpdateJoystickProperties()
    {
        joystickRadiusScaled = joystickImage.rectTransform.lossyScale.x * initialRadiousOfJoystick;
        joystickStartingX = gameObject.transform.position.x;
        joystickStartingY = gameObject.transform.position.y;
    }

    private void UpdatePlayerPosition(Vector2 pressPositionRaw)
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

        //iFlashable.Flash($"{trueXNormalized},{trueYNormalized}");

        userWithMovementInterface.ApplyMovement(trueXNormalized, trueYNormalized);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (userWithMovement.TryGetComponent(out IMovementControl userMovement))
        {
            userWithMovementInterface = userMovement;
        }
        else
        {
            throw new Exception("User has no IMovementControl");
        }

        if (flashText.TryGetComponent(out IFlashable flashable))
        {
            iFlashable = flashable;
        }
        else
        {
            throw new Exception("User has no IFlashable");
        }

        CheckAndUpdateJoystickProperties();
    }

}
