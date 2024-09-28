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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ButtonObserver.currentButtonMode == ButtonMode.LibraryGuide)
        {
            libraryGuideView.SetActive(false);
            controls.SetActive(true);
            ButtonObserver.currentButtonMode = ButtonMode.VirtualLibrary;
        }

    }
}
