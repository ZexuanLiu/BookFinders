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
        Book currentBook = BookManager.Instance.currentBook;
        if (currentBook != null)
        {
            authorText = GameObject.Find("BookAuthorLabel").GetComponent<TextMeshProUGUI>();

            authorText.text = currentBook.Author;
        }
        else
        {
            authorText.text = "No author data available.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
