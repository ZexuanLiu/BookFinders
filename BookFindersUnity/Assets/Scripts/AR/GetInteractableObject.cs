using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetInteractableObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Transform InteractorSource;
    [SerializeField] float InteractRange = 4;

    [SerializeField] GameObject flashText;
    private IFlashableAR iFlashable;

    public void OnPointerClick(PointerEventData eventData)
    {
        TryInteraction();
    }

    void Start()
    {
        if (flashText.TryGetComponent(out IFlashableAR flashable))
        {
            iFlashable = flashable;
        }
        else
        {
            throw new Exception("User has no IFlashable");
        }
    }

    private void TryInteraction()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        bool interactMessageShown = false;
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {

            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractableAR interactObj))
            {
                interactMessageShown = true;
                interactObj.Interact();
                string title = interactObj.GetName();
                string description = interactObj.GetShownText();
                iFlashable.Flash(description);
            }
        }

        if (!interactMessageShown)
        {
            iFlashable.Flash("Not looking at something with extra info...");
        }
    }
}
