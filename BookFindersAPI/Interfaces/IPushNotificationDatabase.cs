using BookFindersLibrary.Models;

namespace BookFindersAPI.Interfaces
{
    public interface IPushNotificationDatabase
    {
        public Task<IEnumerable<PushNotification>> GetPushNotifications();
        public Task<UserLocations> AddLocation(UserLocations locations);
        public Task<PushNotification> AddPushNotification(PushNotification pushNotification);
        public Task<IEnumerable<UserLocations>> GetLocations();
        public Task<IEnumerable<Comment>> GetComments();
        public Task<IEnumerable<Comment>> GetBookComments(string bookId);
        public Task<Comment> AddComment(Comment comment);
        public Task<bool> addThumbsUp(int commentId);
        public Task<bool> subThumbsUp(int commentId);
        public Task<bool> removeComment(int commentId);
        public Task<bool> EditComment(int commentId, string newComment);
    }
}
