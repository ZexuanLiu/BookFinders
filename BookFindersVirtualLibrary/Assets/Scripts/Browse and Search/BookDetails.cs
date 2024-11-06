using System.Collections;
using System.Collections.Generic;
using BookFindersVirtualLibrary.Models;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using UnityEngine.Networking;

public class BookDetails : MonoBehaviour
{
    public TextMeshProUGUI authorText;
    public TextMeshProUGUI publisherText;
    public TextMeshProUGUI publishYearText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI bookDescText;
    public RawImage rawImage;
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
            if (currentBook.ImageLink != "defaultBook.png")
            {
                StartCoroutine(DownloadAndSetImage(currentBook.ImageLink, rawImage));
            }

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

        Screen.orientation = ScreenOrientation.Portrait;
    }

    void OnLaunchVLClicked()
    {
        BookSearchsTracker.SearchResultBooks = BookManager.Instance.SearchResultBooks;
        BookSearchsTracker.SelectedBook = BookManager.Instance.currentBook;
        SceneManager.LoadScene("VirtualLibrary");
    }

    void OnLaunchARClicked()
    {
        BookSearchTracking.SelectedBook = BookManager.Instance.currentBook;
        SceneManager.LoadScene("AR");
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
                rectTransform.sizeDelta = new Vector2(100, 200);
            }
        }
    }
}
