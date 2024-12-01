using Assets.Scripts.Virtual_Library_Scripts.OnscreenControls;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Implement into back and search
interface IActiveBookSearch
{
    public void FinishSearch();
}

public class LocateClicked : MonoBehaviour, IPointerClickHandler, IActiveBookSearch
{
    [SerializeField] GameObject libraryGuideView;
    [SerializeField] GameObject controls;

    [SerializeField] TextMeshProUGUI buttonText;

    [SerializeField] GameObject userPathingObject;
    private IFindingPathTo findingPath;

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
        if (!BookSearchsTracker.BookSearchInProgress)
        {
            StartSearch();
        }
        else
        {
            FinishSearch();
        }
        ButtonObserver.currentButtonMode = ButtonMode.VirtualLibrary;
        libraryGuideView.SetActive(false);
        controls.SetActive(true);
    }

    public void StartSearch()
    {
        string localLocationCode = BookSearchsTracker.SelectedBook.LocationBookShelfNum + BookSearchsTracker.SelectedBook.LocationBookShelfSide;
        buttonText.text = "Finish";
        findingPath.SetBookDestinationTo(localLocationCode, BookSearchsTracker.SelectedBook.Name);
        BookSearchsTracker.BookSearchInProgress = true;
    }

    public void FinishSearch()
    {
        buttonText.text = "Locate";
        findingPath.FinishNavigation();
        BookSearchsTracker.BookSearchInProgress = false;
    }
}
