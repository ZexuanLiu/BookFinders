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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Function to load the scene by name
    public void ChangeScene(string sceneName)
    {
        BookManager.Instance.SetBook(currentBook);
        SceneManager.LoadScene(sceneName);
    }
}
