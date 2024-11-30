using BookFindersAPI.Interfaces;
using BookFindersLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BookFindersAPI.Services
{
    public class ProductionDatabase : DbContext, IDatabase
    {
        private DbSet<PushNotification> _pushNotifications { get; set; }
        private DbSet<Comment> _comment { get; set; }

        private DbSet<UserLocations> _locations { get; set; }

        private DbSet<Coordinate> _coordinates { get; set; }
        private DbSet<UserTrackingInstance> _userTrackingInstances { get; set; }
        private DbSet<UserTrackingSession> _userTrackingSessions { get; set; }

        private DbSet<User> _users { get; set; }
        private DbSet<BookSearchHistory> _bookSearchHistory { get; set; }

        #region User Tracking
        public async Task<UserTrackingSession> SendUserTrackingSession(UserTrackingSession userTrackingSession)
        {
            List<UserTrackingInstance> newUserTrackingInstances = new List<UserTrackingInstance>();
            foreach (UserTrackingInstance userTrackingInstance in userTrackingSession.Locations)
            {
                Coordinate newCoord = new Coordinate()
                {
                    X = userTrackingInstance.Coordinate.X,
                    Y = userTrackingInstance.Coordinate.Y,
                    Z = userTrackingInstance.Coordinate.Z
                };
                _coordinates.Add(newCoord);

                UserTrackingInstance newTrackingInstance = new UserTrackingInstance
                {
                    Coordinate = newCoord,
                    PostDateTime = userTrackingInstance.PostDateTime,
                };
                _userTrackingInstances.Add(newTrackingInstance);
                newUserTrackingInstances.Add(newTrackingInstance);
            }
            _userTrackingSessions.Add(new UserTrackingSession()
            {
                Campus = userTrackingSession.Campus,
                Locations = newUserTrackingInstances,
                TimeEnded = userTrackingSession.TimeEnded,
                TimeStarted = userTrackingSession.TimeStarted,
            });
            await base.SaveChangesAsync();
            return userTrackingSession;
        }
        #endregion

        #region Locations
        public async Task<UserLocations> AddLocation(UserLocations locations)
        {
            _locations.Add(locations);
            await base.SaveChangesAsync();

            return locations;
        }
        public async Task<IEnumerable<UserLocations>> GetLocations()
        {
            return _locations;
        }
        #endregion

        #region Push Notifications
        public async Task<PushNotification> AddPushNotification(PushNotification pushNotification)
        {
            _pushNotifications.Add(pushNotification);
            await base.SaveChangesAsync();
            return pushNotification;
        }
        
        public async Task<IEnumerable<PushNotification>> GetPushNotifications()
        {
            return _pushNotifications;
        }
        #endregion

        #region Comments
        public async Task<IEnumerable<Comment>> GetComments()
        {
            return _comment;
        }
        public async Task<IEnumerable<Comment>> GetBookComments(string bookId)
        {
            return _comment.Where(x => x.BookId == bookId);
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            _comment.Add(comment);
            await base.SaveChangesAsync();

            return comment;
        }
        
        public async Task<bool> addThumbsUp(int commentId)
        {
            var comment = _comment.FirstOrDefault(x=>x.Id==commentId);

            if (comment != null)
            {
             // make Thumbs up ++
            comment.ThumbsUp++;

            // Save the changes to the database
             await base.SaveChangesAsync();
             
            return true;
            
            }
            else{
                return false;
            }
        }
        public async Task<bool> subThumbsUp(int commentId)
        {    
            var comment = _comment.FirstOrDefault(x=>x.Id==commentId);

            if (comment != null)
            {
             // make Thumbs up ++
            comment.ThumbsUp--;

            // Save the changes to the database
             await base.SaveChangesAsync();
             
            return true;
            
            }
            else{
                return false;
            }
        }
        public async Task<bool> removeComment(int commentId){
            var comment = _comment.FirstOrDefault(x=>x.Id==commentId);
             if (comment != null){
                _comment.Remove(comment);
                await base.SaveChangesAsync();
                return true;
             }
             else{
                return false;
            }
        }
        public async Task<bool> EditComment(int commentId, string newComment)
        {    
            var comment = _comment.FirstOrDefault(x=>x.Id==commentId);

            if (comment != null)
            {
             // make Thumbs up ++
            comment.Description = newComment;

            // Save the changes to the database
            await base.SaveChangesAsync();
             
            return true;
            
            }
            else{
                return false;
            }
        }
        #endregion

        #region Login

        public async Task<User> SignUpUser(User newUser)
        {
            _users.Add(newUser);
            await base.SaveChangesAsync();
            return newUser;
        }

        public async Task<User?> GetUserFromUserLogin(UserLogin userLogin)
        {
            User? userLoggingIn = null;
            try
            {
                userLoggingIn = await _users.Include(x => x.UserLogin)
                    .Where(user => user.UserLogin.Username.Equals(userLogin.Username) && user.UserLogin.Password.Equals(userLogin.Password))
                    .FirstAsync();
            }
            catch (InvalidOperationException)
            {
                userLoggingIn = null;
            }

            return userLoggingIn;
        }

        public async Task<IEnumerable<string>> GetUsernames()
        {
            List<string> usernames = new List<string>();
            List<User> users = await _users.Include(x => x.UserLogin).ToListAsync();
            foreach (var user in users)
            {
                usernames.Add(user.UserLogin.Username);
            }

            return usernames;
        }

        #endregion

        #region bookSearchHistory
        public async Task<BookSearchHistory> AddBookSearchHistory(BookSearchHistory bookSearchHistory)
        {
            _bookSearchHistory.Add(bookSearchHistory);
            await base.SaveChangesAsync();

            return bookSearchHistory;
        }
        public async Task<IEnumerable<BookSearchHistory>> GetAllBookSearchHistory()
        {
            return _bookSearchHistory;
        }
        public async Task<bool> RemoveBookSearchHistory(int historyId)
        {
            var bookSearchHistory = _bookSearchHistory.FirstOrDefault(x => x.Id == historyId);
            if (bookSearchHistory != null)
            {
                _bookSearchHistory.Remove(bookSearchHistory);
                await base.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? host = Environment.GetEnvironmentVariable("bookfindersDBHost");
            string? user = Environment.GetEnvironmentVariable("bookfindersDBUser");
            string? pass = Environment.GetEnvironmentVariable("bookfindersDBPassword");
            
            optionsBuilder.UseNpgsql($"Host={host};Database=BookFindersDB;Username={user};Password={pass};SSL Mode=Require;Trust Server Certificate=true");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
