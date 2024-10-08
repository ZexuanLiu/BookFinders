using System.Collections;
using System.Collections.Generic;
using BookFindersVirtualLibrary.Models;
using UnityEngine;
using TMPro;

public class BookDetails : MonoBehaviour
{
    public TextMeshProUGUI authorText;
    // Start is called before the first frame update
    void Start()
    {
        Book currentBook = new Book();
        currentBook.Author = "123";
        if (currentBook != null)
        {
            authorText = GameObject.Find("BookAuthorLabel").GetComponent<TextMeshProUGUI>();

            authorText.text = "Author: " + currentBook.Author;
        }
        else
        {
            authorText.text = "No book data available.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
