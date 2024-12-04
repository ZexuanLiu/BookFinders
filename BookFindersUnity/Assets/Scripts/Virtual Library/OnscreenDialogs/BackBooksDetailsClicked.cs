using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackBooksDetailsClicked : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject bookSearchView;
    [SerializeField] GameObject bookDetailsView;

    [SerializeField] GameObject locateButton;
    private IActiveBookSearch activeBookSearch;

    private void Start()
    {
        if (locateButton.TryGetComponent(out IActiveBookSearch activeSearchInterface))
        {
            activeBookSearch = activeSearchInterface;
        }
        else
        {
            throw new Exception("UserPathing has no IFindingPathTo");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (BookSearchsTracker.BookSearchInProgress)
        {
            BookSearchsTracker.BookSearchInProgress = false;
            activeBookSearch.FinishSearch();
        }
        bookSearchView.SetActive(true);
        bookDetailsView.SetActive(false);
    }

}
