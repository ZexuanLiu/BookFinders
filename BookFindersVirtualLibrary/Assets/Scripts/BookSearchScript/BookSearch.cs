using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BookFindersVirtualLibrary.Models;
using UnityEngine.UI;
using TMPro;

public class BookSearch : MonoBehaviour
{
    public GameObject bookItemPrefab; 
    public Transform contentPanel; 
    public Image searchIcon; 

    private List<Book> bookList = new List<Book>();
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSearchIconClicked()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        DisplayBooks();
    }

    void DisplayBooks()
    {
        foreach (var book in bookList)
        {
            GameObject newBookItem = Instantiate(bookItemPrefab, contentPanel);
            TextMeshProUGUI[] texts = newBookItem.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = book.Name;  
            texts[1].text = book.Author;  
        }
    }
}
