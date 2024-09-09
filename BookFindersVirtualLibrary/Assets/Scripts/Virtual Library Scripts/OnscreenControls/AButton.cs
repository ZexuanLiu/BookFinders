using Assets.Scripts.Virtual_Library_Scripts.OnscreenControls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Transform InteractorSource;
    [SerializeField] float InteractRange = 20;

    [SerializeField] GameObject flashText;
    private IFlashable iFlashable;

    public void OnPointerClick(PointerEventData eventData)
    {
        TryInteraction();
    }

    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteraction();
        }
    }

    private void TryInteraction()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {

            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
                string title = interactObj.GetTitle();
                string description = interactObj.GetDescription();
                iFlashable.Flash(description);
            }
        }
    }


}
