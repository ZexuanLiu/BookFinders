using System.Collections;
using BookFindersVirtualLibrary.Models;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using UnityEngine.Networking;
using UnityEngine.Android;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    private HttpClient client;

    private string bookSearchRecordId = "";
    // Start is called before the first frame update
    async void Start()
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

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            client = new HttpClient(handler);
            //client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer {Environment.GetEnvironmentVariable("bookfindersAPIBearerToken")}");
            client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer -BookFinders-");
            await SaveBookSearchHistory();
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

    async void OnLaunchVLClicked()
    {
        try
        {
            await UpdateBookSearchHistoryNavigationMethod(NavigationMethodEnmu.VirtualLibrary);
        }
        catch (Exception e)
        {
            Debug.Log($"{e}");
        }
        finally
        {
            BookSearchsTracker.SearchResultBooks = BookManager.SearchResultBooks;
            BookSearchsTracker.SelectedBook = BookManager.currentBook;
            BookSearchsTracker.BookSearchInProgress = true;

            SceneManager.LoadScene("VirtualLibrary");
        }
    }

    async void OnLaunchARClicked()
    {


            BookSearchTracking.SelectedBook = BookManager.currentBook;
            BookSearchTracking.BookSearchInProgress = true;
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                errorText.text = "Please enable camera permissions to use Augmented Reality";
                Permission.RequestUserPermission(Permission.Camera);
                return;
            }
            try
            {
                await UpdateBookSearchHistoryNavigationMethod(NavigationMethodEnmu.AugmentedReality);
            }
            catch (Exception e)
            {
                Debug.Log($"{e}");
            }
            finally
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
    public async void OpenOnlineResource()
    {
        Book currentBook = BookManager.currentBook;
        if (currentBook != null)
        {
            try
            {
                await UpdateBookSearchHistoryNavigationMethod(NavigationMethodEnmu.OnlineResources);
            }
            catch (Exception e)
            {
                Debug.Log($"{e}");
            }
            finally
            {
                Application.OpenURL(currentBook.OnlineResourceURL);
            }
        }
    }
    async Task SaveBookSearchHistory()
    {
        HttpResponseMessage response;
        Book currentBook = BookManager.currentBook;
        if (currentBook != null)
        {
            BookSearchHistory bookSearchHistoryObj = new BookSearchHistory();
            bookSearchHistoryObj.Subject = currentBook.Subject;
            bookSearchHistoryObj.NavigationMethod = NavigationMethodEnmu.Unknown;
            string url = $"http://localhost:5156/api/BookSearchHistory/InsertBookSearchHistory";

            switch (currentBook.LibraryCode)
            {
                case "TRAF":
                    bookSearchHistoryObj.Campus = SheridanCampusEnum.Trafalgar;
                    break;
                case "DAV":
                    bookSearchHistoryObj.Campus = SheridanCampusEnum.Davis;
                    break;
                case "HMC":
                    bookSearchHistoryObj.Campus = SheridanCampusEnum.HMC;
                    break;
                default:
                    bookSearchHistoryObj.Campus = SheridanCampusEnum.Unknown;
                    break;
            }

            var json = JsonConvert.SerializeObject(bookSearchHistoryObj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
           
            response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                JObject responseAsJson = JObject.Parse(responseContent);
                
                bookSearchRecordId = responseAsJson["data"]["id"].ToString();
            }
        }
    }
    //update the Navigation method property based on user click vl button or ar button
    async Task UpdateBookSearchHistoryNavigationMethod(NavigationMethodEnmu navigationMethodEnmu)
    {
        HttpResponseMessage response;

        if (bookSearchRecordId != "")
        {
            string url = $"http://localhost:5156/api/BookSearchHistory/editBookSearchHistory/{bookSearchRecordId}/{navigationMethodEnmu}";

            response = await client.PutAsync(url, null);
            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError("Something wrong while update the book search record ");
            }
        }
    }
}