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

    [SerializeField] GameObject userPathingObject;
    private IFindingPathTo findingPath;

    private void Start()
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
        if (BookSearchsTracker.BookSearchInProgress)
        {
            BookSearchsTracker.BookSearchInProgress = false;
            findingPath.FinishNavigation();
        }
        bookSearchView.SetActive(true);
        bookDetailsView.SetActive(false);
    }

}
