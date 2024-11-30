using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using BookFindersLibrary.Models;

namespace BookFindersWebApp.Models
{
	public class DataAnalystModel
	{
        private string URL = "https://localhost:7042";
        public DataAnalystModel()
		{
            string? possibleAPIURL = "http://localhost:5156";//Environment.GetEnvironmentVariable("bookfindersAPIURL");
            if (!string.IsNullOrEmpty(possibleAPIURL))
            {
                URL = possibleAPIURL;
                if (!URL.StartsWith("http"))
                {
                    URL = "http://" + URL;
                }
            }
        }
        public async Task<IEnumerable<BookSearchHistory>> GetBookSearchHistory()
        {
            string subUrl = "/api/BookSearchHistory/getAllBookSearchHistory";

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (HttpClient client = new HttpClient(handler))
            {
                //client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer {Environment.GetEnvironmentVariable("bookfindersAPIBearerToken")}");
                client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer  -BookFinders-");
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
                return fetchedBookSearchHistory;
            }

        }
    }
}

