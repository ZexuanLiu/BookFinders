using System.Collections;
using System.Collections.Generic;
using BookFindersVirtualLibrary.Models;
using UnityEngine;

public class BookManager : MonoBehaviour
{
    //use singleton to save the book currect between different scene
    public static BookManager Instance { get; private set; }
    public Book currentBook;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
