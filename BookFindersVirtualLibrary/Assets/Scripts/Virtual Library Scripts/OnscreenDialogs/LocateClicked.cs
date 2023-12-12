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

    private string initialButtonText;

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

        initialButtonText = buttonText.text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string localLocationCode = BookSearchsTracker.SelectedBook.LocationBookShelfNum + BookSearchsTracker.SelectedBook.LocationBookShelfSide;

        if (!BookSearchsTracker.BookSearchInProgress)
        {
            findingPath.SetBookDestinationTo(localLocationCode, BookSearchsTracker.SelectedBook.Name);
            buttonText.text = "Finish";
        }
        else
        {
            FinishSearch();
        }
        BookSearchsTracker.BookSearchInProgress = !BookSearchsTracker.BookSearchInProgress;
        ButtonObserver.currentButtonMode = ButtonMode.VirtualLibrary;
        libraryGuideView.SetActive(false);
        controls.SetActive(true);
    }

    public void FinishSearch()
    {
        findingPath.FinishNavigation();
        buttonText.text = initialButtonText;
    }
}
