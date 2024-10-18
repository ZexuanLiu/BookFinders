using System.Collections;
using System.Collections.Generic;
using BookFindersVirtualLibrary.Models;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BookDetails : MonoBehaviour
{
    public TextMeshProUGUI authorText;
    public TextMeshProUGUI publisherText;
    public TextMeshProUGUI publishYearText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI bookDescText;
    // Start is called before the first frame update
    void Start()
    {
        Book currentBook = BookManager.Instance.currentBook;
        if (currentBook != null)
        {
            authorText.text = currentBook.Author;
            locationText.text = "Location:"+currentBook.LocationCode;
            publisherText.text = "Publisher:" + currentBook.Publisher;
            publishYearText.text = "Year:" + currentBook.PublishYear;
            bookDescText.text = currentBook.Description;
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
    public void GoToBrowseBooksScene()
    {
        SceneManager.LoadScene("BrowseBooks");
    }
}
