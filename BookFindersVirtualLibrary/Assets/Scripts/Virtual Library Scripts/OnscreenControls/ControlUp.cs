using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlUp : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject userWithMovement;

    private IMovementControl userWithMovementInterface;

    public void OnPointerClick(PointerEventData eventData)
    {
        userWithMovementInterface.ApplyJump();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        userWithMovementInterface.UsingIMovement(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        userWithMovementInterface.UsingIMovement(false);
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
    }
}
