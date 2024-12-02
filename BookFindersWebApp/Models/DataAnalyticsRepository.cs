using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using BookFindersLibrary.Models;

namespace BookFindersWebApp.Models
{
	public class DataAnalyticsRepository
	{
        private string URL = "https://localhost:7042";
        public List<string> TopSubjects { get; set; } = new();
        public List<int> SubjectCounts { get; set; } = new();

        public DataAnalyticsCondition AppliedConditions { get; set; } = new();

        private HttpClientHandler handler;
        HttpClient client;

        public DataAnalyticsRepository()
		{
            string? possibleAPIURL = Environment.GetEnvironmentVariable("bookfindersAPIURL");
            if (!string.IsNullOrEmpty(possibleAPIURL))
            {
                URL = possibleAPIURL;
                if (!URL.StartsWith("http"))
                {
                    URL = "http://" + URL;
                }
            }
            handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            client = new HttpClient(handler);
            //client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer  -BookFinders-");
            client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer {Environment.GetEnvironmentVariable("bookfindersAPIBearerToken")}");

            AppliedConditions = new DataAnalyticsCondition()
            {
                Campus = BookFindersLibrary.Enums.SheridanCampusEnum.All,
                NavigationMethod = BookFindersLibrary.Enums.NavigationMethodEnmu.All,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.Now
            };
        }
        public async Task GetTop5BookSearchHistory()
        {
            string subUrl = "/api/BookSearchHistory/getTop5BookSearchHistory";
            string requestURL = URL + subUrl;
            var response = await client.GetAsync(requestURL);
            var responseString = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(responseString);

            TopSubjects = jsonResponse["data"]?["topSubjects"]?.ToObject<List<string>>() ?? new List<string>();
            SubjectCounts = jsonResponse["data"]?["subjectCounts"]?.ToObject<List<int>>() ?? new List<int>();

            AppliedConditions = new DataAnalyticsCondition()
            {
                Campus = BookFindersLibrary.Enums.SheridanCampusEnum.All,
                NavigationMethod = BookFindersLibrary.Enums.NavigationMethodEnmu.All,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.Now
            };
        }
        //This method is for testing not used in any cshtml page
        public async Task<IEnumerable<BookSearchHistory>> GetBookSearchHistory()
        {
            string subUrl = "/api/BookSearchHistory/getAllBookSearchHistory";
           
            string requestURL = URL + subUrl;
            var response = await client.GetAsync(requestURL);
            var responseString = await response.Content.ReadAsStringAsync();
            List<BookSearchHistory> fetchedBookSearchHistory = new List<BookSearchHistory>();
            JObject responseAsJson = JObject.Parse(responseString);
            if (!responseAsJson.ContainsKey("data"))
            {
                return fetchedBookSearchHistory;
            }

            JArray bookSearchHistoryJson = (JArray)responseAsJson["data"];
            foreach (JObject bookSearchHistoryJsonObj in bookSearchHistoryJson)
            {
                BookSearchHistory bookSearchHistory = JsonConvert.DeserializeObject<BookSearchHistory>(bookSearchHistoryJsonObj.ToString());
                fetchedBookSearchHistory.Add(bookSearchHistory);
            }

            AppliedConditions = new DataAnalyticsCondition()
            {
                Campus = BookFindersLibrary.Enums.SheridanCampusEnum.All,
                NavigationMethod = BookFindersLibrary.Enums.NavigationMethodEnmu.All,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.Now
            };

            return fetchedBookSearchHistory;
        }

        public async Task FilterByDataAnalyticsCondition(DataAnalyticsCondition dataAnalyticsCondition)
        {
            string subUrl = "/api/BookSearchHistory/getTop5BookSearchHistoryWithCondition";

            string requestURL = URL + subUrl;
            var response = await client.PostAsJsonAsync(requestURL, dataAnalyticsCondition);

            var responseString = await response.Content.ReadAsStringAsync();
            JObject jsonResponse = JObject.Parse(responseString);

            TopSubjects = jsonResponse["data"]?["topSubjects"]?.ToObject<List<string>>() ?? new List<string>();
            SubjectCounts = jsonResponse["data"]?["subjectCounts"]?.ToObject<List<int>>() ?? new List<int>();

            AppliedConditions = new DataAnalyticsCondition{
                Campus = dataAnalyticsCondition.Campus,
                NavigationMethod = dataAnalyticsCondition.NavigationMethod,
                StartDate = dataAnalyticsCondition.StartDate == null ? DateTime.MinValue : dataAnalyticsCondition.StartDate.Value,
                EndDate = dataAnalyticsCondition.EndDate == null ? DateTime.Now : dataAnalyticsCondition.EndDate.Value,
            };
        }
    }
}

