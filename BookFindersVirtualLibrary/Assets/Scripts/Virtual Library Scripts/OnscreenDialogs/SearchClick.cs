using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using BookFindersLibrary.Models;
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

    [SerializeField] GameObject searchButton;
    private IHoverableButton searchButtonInterface;

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

        if (searchButton.TryGetComponent(out IHoverableButton iHoverableButton))
        {
            searchButtonInterface = iHoverableButton;
        }
        else
        {
            throw new Exception("SearchButton has no IHoverableButton");
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
        searchStarted = true;
        searchButtonInterface.SetActive();
        scrollBoxControl.ClearSearchingMessage();
        scrollBoxControl.SetSearchingMessage();

        if (!bookSearchView.activeSelf)
        {
            bookSearchView.SetActive(true);
        }
        
        bookDetailsView.SetActive(false);

        string textInput = thisInput.text;
        try
        {
            var response = await client.GetAsync($"https://localhost:7042/api/BookSearch/OnCampus/{textInput}/0");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                JArray foundBooksJson = JArray.Parse(content);

                List<book> foundBooks = new List<book>();
                scrollBoxControl.ClearSearchResults();
                if (foundBooksJson.Count == 0)
                {
                    scrollBoxControl.SetNoResultsFound();
                }

                int index = 0;
                foreach (JToken bookJson in foundBooksJson)
                {
                    book newBook = new book();

                    newBook.Name = bookJson["name"].ToString();
                    newBook.Author = bookJson["author"].ToString();
                    newBook.Description = bookJson["description"].ToString();
                    newBook.LocationCode = bookJson["locationCode"].ToString();
                    newBook.LibraryCode = bookJson["libraryCode"].ToString();
                    newBook.LocationBookShelfNum = (bookJson["locationBookShelfNum"].ToString());
                    newBook.LocationBookShelfSide = bookJson["locationBookShelfSide"].ToString();
                    foundBooks.Add(newBook);

                    string bookName = newBook.Name;
                    string bookAuthor = newBook.Author;

                    scrollBoxControl.AddNewSearchResult(index, bookName, bookAuthor);

                    index++;
                }

                BookSearchsTracker.SearchResultBooks = foundBooks;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"{e}");
        }

        searchStarted = false;
        searchButtonInterface.SetInactive();
        scrollBoxControl.ClearSearchingMessage();
    }


}
