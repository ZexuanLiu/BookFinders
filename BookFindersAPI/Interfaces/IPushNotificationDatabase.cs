using BookFindersLibrary.Models;

namespace BookFindersAPI.Interfaces
{
    public interface IPushNotificationDatabase
    {
        public Task<IEnumerable<PushNotification>> GetPushNotifications();
        
        public Task<PushNotification> AddPushNotification(PushNotification pushNotification);
        public Task<IEnumerable<Comment>> GetComments();
        public Task<Comment> AddComment(Comment comment);
    }
}
