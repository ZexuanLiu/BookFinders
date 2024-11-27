using BookFindersVirtualLibrary.Models;
using UnityEngine;
using UnityEngine.Playables;

namespace Notifications_Manager
{
    public class NotificationController : MonoBehaviour
    {
        private PushNotification _notification;

        public void InitializeNotification(PushNotification notification)
        {
            _notification = notification;
        }
    }
}