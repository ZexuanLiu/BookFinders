using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using BookFindersVirtualLibrary.Models;
using Newtonsoft.Json;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using Newtonsoft.Json.Linq;

public class SearchClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_InputField thisInput;

    [SerializeField] GameObject bookSearchView;
    [SerializeField] GameObject bookDetailsView;

    [SerializeField] GameObject scrollView;
    private IScrollBoxControl scrollBoxControl;

    [SerializeField] GameObject locateButton;
    private IActiveBookSearch activeBookSearch;

    private HttpClient client;
    private volatile bool searchStarted = false;

    void Start()
    {
        if (scrollView.TryGetComponent(out IScrollBoxControl iScrollBox))
        {
            scrollBoxControl = iScrollBox;
        }
        else
        {
            throw new Exception("ScrollControl has no IScrollBoxControl");
        }

        if (locateButton.TryGetComponent(out IActiveBookSearch activeSearchInterface))
        {
            activeBookSearch = activeSearchInterface;
        }
        else
        {
            throw new Exception("UserPathing has no IFindingPathTo");
        }

        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        client = new HttpClient(handler);
    }

    public async void OnPointerClick(PointerEventData eventData)
    {
        if (searchStarted)
        {
            return;
        }

        if (BookSearchsTracker.BookSearchInProgress)
        {
            BookSearchsTracker.BookSearchInProgress = false;
            activeBookSearch.FinishSearch();
        }

        searchStarted = true;
        scrollBoxControl.ClearSearchingMessage();
        scrollBoxControl.SetSearchingMessage();

        if (!bookSearchView.activeSelf)
        {
            bookSearchView.SetActive(true);
        }
        
        bookDetailsView.SetActive(false);

        string textInput = thisInput.text;
        if (string.IsNullOrEmpty(textInput) || textInput.Length == 0)
        {
            searchStarted = false;
            scrollBoxControl.SetNoResultsFound();
            return;
        }

        try
        {
            //var response = await client.GetAsync($"http://localhost:5156/api/BookSearch/OnCampus/{textInput}/0");
            //var response = await client.GetAsync($"https://frp-ask.top:11049/api/BookSearch/OnCampus/{textInput}/0");
            var response = await client.GetAsync($"http://api.krutikov.openstack.fast.sheridanc.on.ca/api/BookSearch/OnCampus/{textInput}/0");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                JArray foundBooksJson = JArray.Parse(content);

                List<Book> foundBooks = new List<Book>();
                scrollBoxControl.ClearSearchResults();
                if (foundBooksJson.Count == 0)
                {
                    scrollBoxControl.SetNoResultsFound();
                }

                int index = 0;
                foreach (JToken bookJson in foundBooksJson)
                {
                    Book newBook = new Book();

                    newBook.Name = bookJson["name"].ToString();
                    newBook.Author = bookJson["author"].ToString();
                    newBook.Description = bookJson["description"].ToString();
                    newBook.LocationCode = bookJson["locationCode"].ToString();
                    newBook.LibraryCode = bookJson["libraryCode"].ToString();
                    newBook.LocationBookShelfNum = (bookJson["locationBookShelfNum"].ToString());
                    newBook.LocationBookShelfSide = bookJson["locationBookShelfSide"].ToString();

                    if (!newBook.LibraryCode.Equals("TRAF"))
                    {
                        continue;
                    }

                    foundBooks.Add(newBook);

                    string bookName = newBook.Name;
                    string bookAuthor = newBook.Author;

                    scrollBoxControl.AddNewSearchResult(index, bookName, bookAuthor);

                    index++;
                }

                BookSearchsTracker.SearchResultBooks = foundBooks;
            }
            else
            {
                scrollBoxControl.SetNoInternetMessage();
            }
        }
        catch (Exception e)
        {
            scrollBoxControl.SetNoInternetMessage();
            Debug.Log($"{e}");
        }

        searchStarted = false;
        scrollBoxControl.ClearSearchingMessage();
    }


}