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
using UnityEngine.UI;
using UnityEngine.Networking;
using Unity.VisualScripting.Antlr3.Runtime;
using System.Net.Http.Headers;
using System.Web;

public class BookSearch : MonoBehaviour, IEndDragHandler
{
    public GameObject bookItemPrefab;
    public Transform contentPanel;
    public Image searchIcon;
    public ScrollRect scrollRect;
    public TMP_Dropdown searchOptionDropdown;

    private HttpClient client;
    private List<Book> foundBooks = new List<Book>();
    private List<Book> allLoadedBooks = new List<Book>();
    private int currentPage = 0;
    public TMP_InputField bookSearchTextArea;
    public TextMeshProUGUI noBookMessage;
    public string BookSearchText { get; set; }
    // Start is called before the first frame update
    void Start()
    {

        Button searchButton = searchIcon.GetComponent<Button>();
        if (searchButton != null)
        {
            searchButton.onClick.AddListener(OnSearchIconClicked);
        }

        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        client = new HttpClient(handler);
        //client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer {Environment.GetEnvironmentVariable("bookfindersAPIBearerToken")}");
        client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer 123");
        noBookMessage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        BookSearchText = HttpUtility.UrlEncode(bookSearchTextArea.text);
    }
    void OnSearchIconClicked()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        DisplayBooks();
    }
    public void OnEndDrag(PointerEventData data)
    {
        currentPage++;
        if (currentPage <= 10)
        {
            if (searchOptionDropdown.value == 0)
            {
                SearchBook(true, currentPage);
            }
            else
            {
                //search for online book
                SearchBook(false, currentPage);
            }
        }
    }
    void DisplayNoBookErrorMessage()
    {
        noBookMessage.gameObject.SetActive(true);
        noBookMessage.text = "No Match Result!";
    }
    async void DisplayBooks()
    {
        try
        {
            currentPage = 0;
            //search for physical book
            if (searchOptionDropdown.value == 0)
            {
                SearchBook(true,currentPage);
            }
            else
            {
                //search for online book
                SearchBook(false, currentPage);
            }
        }
        catch (Exception e)
        {
            Debug.Log($"{e}");
            DisplayNoBookErrorMessage();
        }
    }

    IEnumerator DownloadAndSetImage(string url, RawImage imageComponent)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error downloading image: " + request.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                imageComponent.texture = texture;
                //set the weight and the height of the image
                RectTransform rectTransform = imageComponent.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(120, 150);
            }
        }
    }

    private void CreateBookItem(Book book)
    {
        GameObject newBookItem = Instantiate(bookItemPrefab, contentPanel);
        TextMeshProUGUI[] texts = newBookItem.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = book.Name;
        if (book.LibraryCode == "TRAF")
        {
            texts[1].text = "Campus: Trafalgar";
        }
        else if (book.LibraryCode == "DAV")
        {
            texts[1].text = "Campus: Davis";
        }
        else if (book.LibraryCode == "HMC")
        {
            texts[1].text = "Campus: HMC";
        }
        else
        {
            texts[1].text = "Unknown Campus";
        }

        texts[2].text = book.Author;

        if (book.ImageLink != "defaultBook.png")
        {
            RawImage imageComponent = newBookItem.GetComponentInChildren<RawImage>();
            StartCoroutine(DownloadAndSetImage(book.ImageLink, imageComponent));
        }

        BookItemController controller = newBookItem.GetComponent<BookItemController>();
        controller.Initialize(book);
    }
    private async void SearchBook(bool isPhysicalBook, int page)
    {
        HttpResponseMessage response;
        if (isPhysicalBook)
        {
            //response = await client.GetAsync($"http://137.184.5.147:4004/api/BookSearch/OnCampus/{BookSearchText}/{page}");
            response = await client.GetAsync($"http://localhost:5156/api/BookSearch/OnCampus/{BookSearchText}/{page}");
        
        }
        else
        {
           //response = await client.GetAsync($"http://137.184.5.147:4004/api/BookSearch/{BookSearchText}/{page}");
            response = await client.GetAsync($"http://localhost:5156/api/BookSearch/{BookSearchText}/{page}");
        }

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            JArray foundBooksJson = JArray.Parse(content);

            foundBooks = new List<Book>();

            int index = 0;
            foreach (JToken bookJson in foundBooksJson)
            {
                Book newBook = new Book();

                newBook.Name = bookJson["name"].ToString();
                newBook.Author = bookJson["author"].ToString();
                newBook.Description = bookJson["description"].ToString();
                newBook.ImageLink = bookJson["imageLink"].ToString();
                newBook.Isbns = string.Join(", ", bookJson["isbns"].ToObject<string[]>());
                newBook.Subject = bookJson["subject"].ToString();
                newBook.Publisher = bookJson["publisher"].ToString();
                newBook.PublishYear = bookJson["publishYear"].ToString();
                newBook.LocationCode = bookJson["locationCode"].ToString();
                newBook.LibraryCode = bookJson["libraryCode"].ToString();
                newBook.LocationBookShelfNum = bookJson["locationBookShelfNum"].ToString();
                newBook.LocationBookShelfSide = bookJson["locationBookShelfSide"].ToString();
                newBook.OnlineResourceURL = bookJson["onlineResourceURL"].ToString();

                //if (!newBook.LibraryCode.Equals("TRAF"))
                //{
                //    continue;
                //}

                foundBooks.Add(newBook);

                index++;
            }
            if (foundBooks.Count == 0)
            {
                //if no book found enable the error message
                DisplayNoBookErrorMessage();
                return;
            }
            //if books found disable the error message
            noBookMessage.gameObject.SetActive(false);

            foreach (var book in foundBooks)
            {
                CreateBookItem(book);
            }
            allLoadedBooks.AddRange(foundBooks);
            BookManager.Instance.SearchResultBooks = foundBooks;
        }
        else
        {
            DisplayNoBookErrorMessage();
        }
    }
}