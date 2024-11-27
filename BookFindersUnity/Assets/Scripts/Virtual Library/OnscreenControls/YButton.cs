using Assets.Scripts.Virtual_Library_Scripts.OnscreenControls;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class YButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject libraryGuideView;
    [SerializeField] GameObject bookSearchView;
    [SerializeField] GameObject bookDetailedView;
    [SerializeField] GameObject controls;

    // Start is called before the first frame update
    void Start()
    {
        libraryGuideView.SetActive(false);
        bookSearchView.SetActive(false);
        bookDetailedView.SetActive(BookSearchsTracker.SelectedBook!=null);

        ButtonObserver.currentButtonMode = ButtonMode.VirtualLibrary;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ButtonObserver.currentButtonMode == ButtonMode.VirtualLibrary || ButtonObserver.currentButtonMode == ButtonMode.Menu)
        {
            ButtonObserver.currentButtonMode = ButtonMode.LibraryGuide;
            libraryGuideView.SetActive(true);
            controls.SetActive(false);

            if (bookSearchView.activeSelf == false && bookDetailedView.activeSelf == false)
            {
                bookSearchView.SetActive(true);
            }
        }
        else if (ButtonObserver.currentButtonMode == ButtonMode.LibraryGuide)
        {
            ButtonObserver.currentButtonMode = ButtonMode.VirtualLibrary;
            libraryGuideView.SetActive(false);
            controls.SetActive(true);
        }
        else
        {
            Debug.Log($"Unaccounted for Button Mode: {ButtonObserver.currentButtonMode}");
        }
    }

}
