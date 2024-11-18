using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlDown : MonoBehaviour, IPointerClickHandler
{

    public GameObject userAndCameraObject;

    private bool isCrouching;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCrouching)
        {
            userAndCameraObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            userAndCameraObject.transform.localScale = new Vector3(1, 0.5f, 1);
        }
        isCrouching = !isCrouching;
    }


}
