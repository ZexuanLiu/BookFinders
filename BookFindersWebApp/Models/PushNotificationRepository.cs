using BookFindersLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace BookFindersWebApp.Models
{
    public class PushNotificationRepository
    {
        private static readonly string URL = "https://localhost:7042";


        static PushNotificationRepository()
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

        public async static Task<IEnumerable<PushNotification>> GetAllPushNotifications()
        {
            string subUrl = "/api/PushNotification/getPushNotifications";

            using (HttpClient client = new HttpClient())
            {
                string requestURL = URL + subUrl;
                var response = await client.GetAsync(requestURL);
                var responseString = await response.Content.ReadAsStringAsync();

                List<PushNotification> fetchedPushNotifications = new List<PushNotification>();
                JObject responseAsJson = JObject.Parse(responseString);
                if (!responseAsJson.ContainsKey("data"))
                {
                    return fetchedPushNotifications;
                }

                JArray pushNotificationsJson = (JArray)responseAsJson["data"];
                foreach (JObject pushNotificationJson in pushNotificationsJson)
                {
                    PushNotification pushNotification = JsonConvert.DeserializeObject<PushNotification>(pushNotificationJson.ToString());
                    fetchedPushNotifications.Add(pushNotification);
                }
                return fetchedPushNotifications;
            }

        }

        public async static Task<bool> AddPushNotification(PushNotification pushNotification)
        {
            string subUrl = "/api/PushNotification/sendPushNotification";

            using (HttpClient client = new HttpClient())
            {
                string requestURL = URL + subUrl;
                var response = await client.PostAsJsonAsync(requestURL, pushNotification);

                return response.IsSuccessStatusCode;
            }
        }
    }
}
