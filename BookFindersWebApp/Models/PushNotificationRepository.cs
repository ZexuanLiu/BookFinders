using BookFindersLibrary.Models;

namespace BookFindersWebApp.Models
{
    public class PushNotificationRepository
    {
        public static List<PushNotification> _pushNotifications = new List<PushNotification>();
        private static int _IdCounter = 0;

        static PushNotificationRepository()
        {
            LoadDefaultPushNotificationHistoryAndReset();
        }

        public static IEnumerable<PushNotification> GetAllPushNotifications()
        {
            return _pushNotifications;
        }

        public static bool AddPushNotification(PushNotification pushNotification)
        {
            pushNotification.Id = _IdCounter++;
            _pushNotifications.Add(pushNotification);
            return true;
        }

        private static void LoadDefaultPushNotificationHistoryAndReset()
        {
            _pushNotifications.Clear();
            _IdCounter = 0;

            _pushNotifications = new List<PushNotification> {
                new PushNotification()
                {
                    Id = _IdCounter++,
                    Title = "Snow Day Tomorrow",
                    Description = "Due to the harsh weather, Sheridan College will be closed tomorrow, stay safe!",
                    StartDateTime = DateTime.Today,
                    EndDateTime = DateTime.Today.AddDays(1)
                },
                new PushNotification()
                {
                    Id = _IdCounter++,
                    Title = "Book Festival Next Week!",
                    Description = "Remember to attend the Sheridan Book Festival next week! Lots of prizes and food available!",
                    StartDateTime = DateTime.Today,
                    EndDateTime = DateTime.Today.AddDays(7)
                },
            };
        }
    }
}
