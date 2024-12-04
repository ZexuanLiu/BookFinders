using BookFindersLibrary.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace BookFindersWebApp.Models
{
    public class LoginRepository
    {
        private static readonly string URL = "https://localhost:7042";

        static LoginRepository()
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
        }

        public async static Task<User?> Login(UserLogin userLogin)
        {
            string subUrl = "/api/Login/Login";

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("X-Authorization", $"Bearer {Environment.GetEnvironmentVariable("bookfindersAPIBearerToken")}");
                string requestURL = URL + subUrl;

                var json = JsonConvert.SerializeObject(userLogin);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(requestURL, content);
                var responseString = await response.Content.ReadAsStringAsync();

                User? user = null;
                JObject responseAsJson = JObject.Parse(responseString);
                if (!responseAsJson.ContainsKey("data"))
                {
                    return user;
                }

                if ((int)responseAsJson["status"] != 200)
                {
                    return user;
                }

                JObject possibleUser = (JObject)responseAsJson["data"];
                if (possibleUser != null)
                {
                    user = possibleUser.ToObject<User>();
                }
                return user;

            }
        }
    }
}
