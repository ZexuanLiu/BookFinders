using System;
using System.Collections.Generic;
using System.Net.Http;
using BookFindersVirtualLibrary.Models;
using Newtonsoft.Json.Linq;
using Notifications_Manager;
using TMPro;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    public GameObject notificationPrefab;
    public Transform content;
    
    private HttpClient client;
    
    // Start is called before the first frame update
    void Start()
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (receiver, cert, chain, sslPolicyErrors) => true;
        client = new HttpClient(handler);
        
        ShowNotifications();
    }

    async void ShowNotifications()
    {
        try
        {
            var response = await client.GetAsync($"http://localhost:5156/api/PushNotification/getPushNotifications");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                JObject notificationsObj = JObject.Parse(data);

                if (notificationsObj["data"] != null && notificationsObj["data"] is JArray notificationsArr)
                {
                    List<PushNotification> notifications = new List<PushNotification>();

                    foreach (JToken notificationTkn in notificationsArr)
                    {
                        PushNotification pushNotification = new PushNotification();

                        pushNotification.Title = notificationTkn["title"].ToString();
                        pushNotification.Description = notificationTkn["description"].ToString();
                    
                        notifications.Add(pushNotification);
                    }

                    foreach (var notification in notifications)
                    {
                        GameObject notificationArea = Instantiate(notificationPrefab, content);
                        TMP_Text[] textArr = notificationArea.GetComponentsInChildren<TMP_Text>();

                        textArr[0].text = notification.Title;
                        textArr[1].text = notification.Description;
                        
                        NotificationController controller = notificationArea.GetComponent<NotificationController>();
                        controller.InitializeNotification(notification);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"{e}");
        }
    }
}
