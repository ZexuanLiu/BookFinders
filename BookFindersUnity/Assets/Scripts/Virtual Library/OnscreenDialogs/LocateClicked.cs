using Assets.Scripts.Virtual_Library_Scripts.OnscreenControls;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using BookFindersVirtualLibrary.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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

    private HttpClient client;

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

        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer $B34R4RT0K3N$_for_-BookFinders-");
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

    public async void StartSearch()
    {
        string localLocationCode = BookSearchsTracker.SelectedBook.LocationBookShelfNum + BookSearchsTracker.SelectedBook.LocationBookShelfSide;
        buttonText.text = "Finish";
        findingPath.SetBookDestinationTo(localLocationCode, BookSearchsTracker.SelectedBook.Name);
        BookSearchsTracker.BookSearchInProgress = true;

        await UpdateBookSearchHistoryNavigationMethod(NavigationMethodEnmu.VirtualLibrary);
    }

    public void FinishSearch()
    {
        buttonText.text = "Locate";
        findingPath.FinishNavigation();
        BookSearchsTracker.BookSearchInProgress = false;
    }

    async Task UpdateBookSearchHistoryNavigationMethod(NavigationMethodEnmu navigationMethodEnmu)
    {
        HttpResponseMessage response;

        if (!string.IsNullOrEmpty(BookSearchsTracker.BookSearchRecordId))
        {
            string url = $"http://137.184.5.147:4004/api/BookSearchHistory/editBookSearchHistory/{BookSearchsTracker.BookSearchRecordId}/{navigationMethodEnmu}";
            response = await client.PutAsync(url, null);
            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError("Something went wrong while updating the book search record");
            }
        }
    }
}
