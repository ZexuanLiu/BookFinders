using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlLeftRight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ControlType controlType;

    public Camera mainCamera;

    public static volatile bool leftClicked = false;
    public static volatile bool rightClicked = false;

    private static float maxFOV = 90f;
    private static float minFOV = 40f;
    private static float zoomSpeed = 30f;

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (controlType)
        {
            case (ControlType.Left):
                {
                    leftClicked = false;
                    break;
                }
            case (ControlType.Right):
                {
                    rightClicked = false;
                    break;
                }

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (controlType)
        {
            case (ControlType.Left):
                {
                    leftClicked = true;
                    break;
                }
            case (ControlType.Right):
                {
                    rightClicked = true;
                    break;
                }
        }
    }



    // Update is called once per frame
    void Update()
    {
        float amountToChange = 0;
        if (leftClicked ^ rightClicked) {
            if (leftClicked && mainCamera.fieldOfView < maxFOV)
            {
                amountToChange = zoomSpeed;
            }
            else if (rightClicked && mainCamera.fieldOfView > minFOV)
            {
                amountToChange = zoomSpeed * -1;
            }

            mainCamera.fieldOfView += (amountToChange * Time.deltaTime);
        }

    }

}

public enum ControlType
{
    Left,
    Right
}
