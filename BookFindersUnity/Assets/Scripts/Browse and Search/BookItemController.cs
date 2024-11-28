using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BookFindersVirtualLibrary.Models;

public class BookItemController : MonoBehaviour
{
    private Book currentBook;

    public void Initialize(Book book)
    {
        currentBook = book;
    }

    // Function to load the scene by name
    public void ChangeScene(string sceneName)
    {
        BookManager.currentBook = currentBook;
        SceneManager.LoadScene(sceneName);
    }
}
