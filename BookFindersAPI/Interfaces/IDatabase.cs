using BookFindersLibrary.Models;

namespace BookFindersAPI.Interfaces
{
    public interface IDatabase
    {

        #region User Tracking
        public Task<UserTrackingSession> SendUserTrackingSession(UserTrackingSession userTrackingSession);
        #endregion

        #region Locations
        public Task<UserLocations> AddLocation(UserLocations locations);
        public Task<IEnumerable<UserLocations>> GetLocations();
        #endregion

        #region Push Notifications
        public Task<IEnumerable<PushNotification>> GetPushNotifications();
        public Task<PushNotification> AddPushNotification(PushNotification pushNotification);
        #endregion

        #region Comments
        public Task<IEnumerable<Comment>> GetComments();
        public Task<IEnumerable<Comment>> GetBookComments(string bookId);
        public Task<Comment> AddComment(Comment comment);
        public Task<bool> addThumbsUp(int commentId);
        public Task<bool> subThumbsUp(int commentId);
        public Task<bool> removeComment(int commentId);
        public Task<bool> EditComment(int commentId, string newComment);
        #endregion

        #region Login
        public Task<User> SignUpUser(User newUser);

        public Task<User?> GetUserFromUserLogin(UserLogin userLogin);

        public Task<IEnumerable<string>> GetUsernames();
        #endregion
    }
}
