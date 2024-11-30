using System.Collections;
using System.Collections.Generic;
using BookFindersVirtualLibrary.Models;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using UnityEngine.Networking;
using UnityEngine.Android;

public class BookDetails : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI authorText;
    public TextMeshProUGUI publisherText;
    public TextMeshProUGUI publishYearText;
    public TextMeshProUGUI locationText;
    public TextMeshProUGUI bookDescText;
    public TextMeshProUGUI campusText;
    public RawImage rawImage;
    
    public GameObject gameObjectBtnLaunchVL;
    public GameObject gameObjectBtnLaunchAR;

    public GameObject gameObjectBtnBrowser;
    public GameObject gameObjectLblBrowser;

    public TextMeshProUGUI errorText;

    // Start is called before the first frame update
    void Start()
    {
        Book currentBook = BookManager.currentBook;
        errorText.text = "";

        if (currentBook != null)
        {
            titleText.text = currentBook.Name;
            authorText.text = currentBook.Author;
            locationText.text = "Location:"+currentBook.LocationCode;
            publisherText.text = "Publisher:" + currentBook.Publisher;
            publishYearText.text = "Year:" + currentBook.PublishYear;
            bookDescText.text = currentBook.Description;
            if (currentBook.LibraryCode == "TRAF")
            {
                campusText.text = "Campus: TRAF";
            }
            else if (currentBook.LibraryCode == "DAV")
            {
                campusText.text = "Campus: DAV";
            }
            else if (currentBook.LibraryCode == "HMC")
            {
                campusText.text = "Campus: HMC";
            }
            else
            {
                campusText.text = "Unknown Campus";
            }
            if (currentBook.ImageLink != "defaultBook.png")
            {
                StartCoroutine(DownloadAndSetImage(currentBook.ImageLink, rawImage));
            }

        }
        else
        {
            titleText.text = "No title data available.";
            authorText.text = "No author data available.";
            locationText.text = "Location: (Unknown)";
            publisherText.text = "Publisher: (Unknown)";
            publishYearText.text = "Year: (Unknown)";
            bookDescText.text = "No Description";
            campusText.text = "Campus: (Unknown)";
            
        }

        Button btnLaunchVL = gameObjectBtnLaunchVL.GetComponent<Button>();
        if (btnLaunchVL != null)
        {
            if (currentBook.LibraryCode != "TRAF")
            {
                btnLaunchVL.interactable = false;
            }
            else
            {
                btnLaunchVL.interactable = true;
                btnLaunchVL.onClick.AddListener(OnLaunchVLClicked);
            }        
        }

        Button btnLaunchAR = gameObjectBtnLaunchAR.GetComponent<Button>();
        if (btnLaunchAR != null)
        {
            if (currentBook.LibraryCode != "TRAF")
            {
                btnLaunchAR.interactable = false;
            }
            else
            {
                btnLaunchAR.interactable = true;
                btnLaunchAR.onClick.AddListener(OnLaunchARClicked);
            }
        }

        Button btnOpenOnlineResource = gameObjectBtnBrowser.GetComponent<Button>();
        if (btnOpenOnlineResource != null)
        {
            if (string.IsNullOrWhiteSpace(currentBook.OnlineResourceURL))
            {
                btnOpenOnlineResource.GetComponent<Image>().enabled = false;
                gameObjectLblBrowser.SetActive(false);
            }
            else
            {
                btnOpenOnlineResource.GetComponent<Image>().enabled = true;
                gameObjectLblBrowser.SetActive(true);
                btnOpenOnlineResource.onClick.AddListener(OpenOnlineResource);
            }
        }
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void OnLaunchVLClicked()
    {
        BookSearchsTracker.SearchResultBooks = BookManager.SearchResultBooks;
        BookSearchsTracker.SelectedBook = BookManager.currentBook;
        BookSearchsTracker.BookSearchInProgress = true;

        SceneManager.LoadScene("VirtualLibrary");
    }

    void OnLaunchARClicked()
    {
        BookSearchTracking.SelectedBook = BookManager.currentBook;
        BookSearchTracking.BookSearchInProgress = true;

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            errorText.text = "Please enable camera permissions to use Augmented Reality";
            Permission.RequestUserPermission(Permission.Camera);
        }
        else
        {
            SceneManager.LoadScene("AR");
        }
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

                float imageScaleFactor = 300.0f / texture.height;
                rectTransform.sizeDelta = new Vector2((float)texture.width * imageScaleFactor, (float)texture.height * imageScaleFactor);
                var test = rectTransform.sizeDelta;
            }
        }
    }
    public void OpenOnlineResource()
    {
        Book currentBook = BookManager.currentBook;
        if (currentBook != null)
        {
            Application.OpenURL(currentBook.OnlineResourceURL);
        }
    }
}
