using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using BookFindersVirtualLibrary.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class BookDetailsControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshName;
    [SerializeField] TextMeshProUGUI textMeshAuthor;
    [SerializeField] TextMeshProUGUI textMeshLocationCode;
    [SerializeField] TextMeshProUGUI textMeshDescription;
    [SerializeField] TextMeshProUGUI textMeshISBNs;

    private HttpClient client;

    // Start is called before the first frame update
    async void Start()
    {
        if (BookSearchsTracker.SelectedBook == null)
        {
            return;
        }
        textMeshName.text = BookSearchsTracker.SelectedBook.Name;
        textMeshAuthor.text = BookSearchsTracker.SelectedBook.Author;
        textMeshLocationCode.text = BookSearchsTracker.SelectedBook.LocationCode;
        textMeshDescription.text = BookSearchsTracker.SelectedBook.Description;
        textMeshISBNs.text = string.Join(',',BookSearchsTracker.SelectedBook.Isbns);

        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer {Environment.GetEnvironmentVariable("bookfindersAPIBearerToken")}");
    }

    async void OnEnable()
    {
        if (BookSearchsTracker.SelectedBook == null)
        {
            return;
        }
        textMeshName.text = BookSearchsTracker.SelectedBook.Name;
        textMeshAuthor.text = BookSearchsTracker.SelectedBook.Author;
        textMeshLocationCode.text = BookSearchsTracker.SelectedBook.LocationCode;
        textMeshDescription.text = BookSearchsTracker.SelectedBook.Description;
        textMeshISBNs.text = string.Join(',', BookSearchsTracker.SelectedBook.Isbns);

        await SaveBookSearchHistory();
    }

    async Task SaveBookSearchHistory()
    {
        HttpResponseMessage response;
        Book currentBook = BookSearchsTracker.SelectedBook;
        if (currentBook != null)
        {
            BookSearchHistory bookSearchHistoryObj = new BookSearchHistory();
            bookSearchHistoryObj.Subject = currentBook.Subject == null ? "Unknown" : currentBook.Subject;
            bookSearchHistoryObj.NavigationMethod = NavigationMethodEnmu.Unknown;
            string url = $"http://137.184.5.147:4004/api/BookSearchHistory/InsertBookSearchHistory";
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

                BookSearchsTracker.BookSearchRecordId = responseAsJson["data"]["id"].ToString();
            }
        }

    }
}
