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


public class BookSearch : MonoBehaviour
{
    public GameObject bookItemPrefab; 
    public Transform contentPanel; 
    public Image searchIcon;
    private HttpClient client;
    private List<Book> bookList = new List<Book>();
    public TMP_InputField bookSearchTextArea;
    public TextMeshProUGUI noBookMessage;
    public string BookSearchText { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Book bookObj = new Book();
        Book bookObj2 = new Book();
        bookObj.Name = "testBook";
        bookObj.Author = "testAuthor";
        bookObj.Description = "123";
        bookObj2.Name = "testBook2";
        bookObj2.Author = "testAuthor2";
        bookObj2.Description = "1234";
        bookList.Add(bookObj);
        bookList.Add(bookObj);
        bookList.Add(bookObj2);
        bookList.Add(bookObj2);

        Button searchButton = searchIcon.GetComponent<Button>();
        if (searchButton != null)
        {
            searchButton.onClick.AddListener(OnSearchIconClicked);
        }

        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        client = new HttpClient(handler);
        noBookMessage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        BookSearchText = bookSearchTextArea.text;
    }

    void OnSearchIconClicked()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        DisplayBooks();
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
            var response = await client.GetAsync($"http://localhost:5156/api/BookSearch/OnCampus/{BookSearchText}/0");
            //var response = await client.GetAsync($"https://frp-ask.top:11049/api/BookSearch/OnCampus/{BookSearchText}/0");
            //var response = await client.GetAsync($"http://api.krutikov.openstack.fast.sheridanc.on.ca/api/BookSearch/OnCampus/{BookSearchText}/0");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                JArray foundBooksJson = JArray.Parse(content);

                List<Book> foundBooks = new List<Book>();

                int index = 0;
                foreach (JToken bookJson in foundBooksJson)
                {
                    Book newBook = new Book();

                    newBook.Name = bookJson["name"].ToString();
                    newBook.Author = bookJson["author"].ToString();
                    newBook.Description = bookJson["description"].ToString();
                    newBook.Publisher = bookJson["publisher"].ToString();
                    newBook.PublishYear = bookJson["publishYear"].ToString();
                    newBook.LocationCode = bookJson["locationCode"].ToString();
                    newBook.LibraryCode = bookJson["libraryCode"].ToString();
                    newBook.LocationBookShelfNum = (bookJson["locationBookShelfNum"].ToString());
                    newBook.LocationBookShelfSide = bookJson["locationBookShelfSide"].ToString();

                    if (!newBook.LibraryCode.Equals("TRAF"))
                    {
                        continue;
                    }  

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
                string url = "https://picsum.photos/id/237/200/300";
                foreach (var book in foundBooks)
                {
                    GameObject newBookItem = Instantiate(bookItemPrefab, contentPanel);
                    TextMeshProUGUI[] texts = newBookItem.GetComponentsInChildren<TextMeshProUGUI>();
                    texts[0].text = book.Name;
                    texts[1].text = book.Author;

                    RawImage imageComponent = newBookItem.GetComponentInChildren<RawImage>();
                    StartCoroutine(DownloadAndSetImage(url, imageComponent));

                    BookItemController controller = newBookItem.GetComponent<BookItemController>();
                    controller.Initialize(book);

                }

                BookManager.Instance.SearchResultBooks = foundBooks;
            }
            else
            {
                DisplayNoBookErrorMessage();
            }
        }
        catch (Exception e)
        {
            Debug.Log($"{e}");
            DisplayNoBookErrorMessage();
        }
        //foreach (var book in bookList)
        //{
        //    GameObject newBookItem = Instantiate(bookItemPrefab, contentPanel);
        //    TextMeshProUGUI[] texts = newBookItem.GetComponentsInChildren<TextMeshProUGUI>();
        //    texts[0].text = book.Name;
        //    texts[1].text = book.Author;

        //    BookItemController controller = newBookItem.GetComponent<BookItemController>();
        //    controller.Initialize(book);

        //}
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
}
