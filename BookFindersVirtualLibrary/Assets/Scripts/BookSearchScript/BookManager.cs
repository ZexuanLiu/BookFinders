using System.Collections;
using System.Collections.Generic;
using BookFindersVirtualLibrary.Models;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    public static BookManager Instance { get; private set; }
    public Book currentBook;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void SetBook(Book book)
    {
        currentBook = book;
    }

    public Book GetBook()
    {
        return currentBook;
    }
}
