using Assets.Scripts.Virtual_Library_Scripts.OnscreenControls;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Implement into back and search
interface ActiveBookSearch
{
    public void FinishSearch();
}

public class LocateClicked : MonoBehaviour, IPointerClickHandler, ActiveBookSearch
{
    [SerializeField] GameObject userPathingObject;
    [SerializeField] GameObject libraryGuideView;
    [SerializeField] GameObject controls;

    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject selfHoverableButton;

    private IFindingPathTo findingPath;
    private IHoverableButton hoverableButton;

    private bool bookSearchActive = false;
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

        if (selfHoverableButton.TryGetComponent(out IHoverableButton iHoverableButton))
        {
            hoverableButton = iHoverableButton;
        }
        else
        {
            throw new Exception("LocateClicked has no IHoverableButton");
        }

        initialButtonText = buttonText.text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string localLocationCode = BookSearchsTracker.SelectedBook.LocationBookShelfNum + BookSearchsTracker.SelectedBook.LocationBookShelfSide;

        if (!bookSearchActive)
        {
            findingPath.SetBookDestinationTo(localLocationCode, BookSearchsTracker.SelectedBook.Name);
            buttonText.text = "Finish";
        }
        else
        {
            findingPath.FinishNavigation();
            buttonText.text = initialButtonText;
        }
        bookSearchActive = !bookSearchActive;
        libraryGuideView.SetActive(false);
        ButtonObserver.currentButtonMode = ButtonMode.VirtualLibrary;
        controls.SetActive(true);
        hoverableButton.SetInactive();
    }

    public void FinishSearch()
    {
        findingPath.FinishNavigation();
    }
}