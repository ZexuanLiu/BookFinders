using Assets.Scripts.Virtual_Library_Scripts.OnscreenControls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Close : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject libraryGuideView;
    [SerializeField] GameObject controls;

    [SerializeField] GameObject selfHoverableButton = null;
    private IHoverableButton hoverableButton;

    void Start()
    {
        if (selfHoverableButton != null)
        {
            if (selfHoverableButton.TryGetComponent(out IHoverableButton iHoverableButton))
            {
                hoverableButton = iHoverableButton;
            }
            else
            {
                throw new Exception("LocateClicked has no IHoverableButton");
            }
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ButtonObserver.currentButtonMode == ButtonMode.LibraryGuide)
        {
            libraryGuideView.SetActive(false);
            controls.SetActive(true);
            ButtonObserver.currentButtonMode = ButtonMode.VirtualLibrary;
            if (selfHoverableButton != null)
            {
                hoverableButton.SetInactive();
            }
        }

    }
}
