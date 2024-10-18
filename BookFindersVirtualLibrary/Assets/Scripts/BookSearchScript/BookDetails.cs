using System.Collections;
using System.Collections.Generic;
using BookFindersVirtualLibrary.Models;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BookDetails : MonoBehaviour
{
    public TextMeshProUGUI authorText;
    public TextMeshProUGUI publisherText;
    public TextMeshProUGUI publishYearText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI bookDescText;

    public GameObject gameObjectBtnLaunchVL;
    public GameObject gameObjectBtnLaunchAR;

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

        Button btnLaunchVL = gameObjectBtnLaunchVL.GetComponent<Button>();
        if (btnLaunchVL != null)
        {
            btnLaunchVL.onClick.AddListener(OnLaunchVLClicked);
        }

        Button btnLaunchAR = gameObjectBtnLaunchAR.GetComponent<Button>();
        if (btnLaunchAR != null)
        {
            btnLaunchAR.onClick.AddListener(OnLaunchARClicked);
        }
    }

    void OnLaunchVLClicked()
    {
        SceneManager.LoadScene("Virtual Library");
    }

    void OnLaunchARClicked()
    {
        BookSearchTracking.SelectedBook = BookManager.Instance.currentBook;
        SceneManager.LoadScene("AR");
    }

    public void GoToBrowseBooksScene()
    {
        SceneManager.LoadScene("BrowseBooks");
    }
}
