using Assets.Scripts.Virtual_Library_Scripts.OnscreenControls;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Implement into back and search
interface ActvieBookSearch
{
    public void FinishSearch();
}

public class LocateClicked : MonoBehaviour, IPointerClickHandler, ActvieBookSearch
{
    [SerializeField] GameObject userPathingObject;
    [SerializeField] GameObject libraryGuideView;
    [SerializeField] GameObject controls;

    private IFindingPathTo findingPath;

    private bool bookSearchActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if (userPathingObject.TryGetComponent(out IFindingPathTo findingPathInterface))
        {
            findingPath = findingPathInterface;
        }
        else
        {
            throw new Exception("UserPathing has no IFindingPathTo");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!bookSearchActive)
        {
            findingPath.SetBookDestinationTo(BookSearchsTracker.BookLocationCode);
        }
        else
        {
            findingPath.FinishNavigation();
        }
        bookSearchActive = !bookSearchActive;
        libraryGuideView.SetActive(false);
        ButtonObserver.currentButtonMode = ButtonMode.VirtualLibrary;
        controls.SetActive(true);

    }

    public void FinishSearch()
    {
        findingPath.FinishNavigation();
    }
}
