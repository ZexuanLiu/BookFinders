using BookFindersAPI.Interfaces;
using BookFindersLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookFindersAPI.Services
{
    public class TestDatabase : DbContext, IDatabase
    {
        private DbSet<PushNotification> _pushNotifications { get; set; }
         private DbSet<Comment> _comment { get; set; }


        public async Task<PushNotification> AddPushNotification(PushNotification pushNotification)
        {
            _pushNotifications.Add(pushNotification);
            await base.SaveChangesAsync();

            return pushNotification;
        }
        public async Task<Comment> AddComment(Comment comment)
        {
            _comment.Add(comment);
            await base.SaveChangesAsync();

            return comment;
        }
        public async Task<IEnumerable<Comment>> GetComments()
        {
            return _comment;
        }
        public async Task<IEnumerable<Comment>> GetBookComments(string bookId)
        {
            return _comment.Where(x=>x.BookId == bookId);
        }
        public async Task<IEnumerable<PushNotification>> GetPushNotifications()
        {
            return _pushNotifications;
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

        public async Task<bool> LoadDefaultPushNotificationHistoryAndReset()
        {
            foreach (var pushNotification in _pushNotifications)
            {
                _pushNotifications.Remove(pushNotification);
            }
            await base.SaveChangesAsync();

            _pushNotifications.Add(
                new PushNotification()
                {
                    Title = "Snow Day Tomorrow",
                    Description = "Due to the harsh weather, Sheridan College will be closed tomorrow, stay safe!",
                    StartDateTime = DateTime.Today,
                    EndDateTime = DateTime.Today.AddDays(1)
                });

            _pushNotifications.Add(
                new PushNotification()
                {
                    Title = "Book Festival Next Week!",
                    Description = "Remember to attend the Sheridan Book Festival next week! Lots of prizes and food available!",
                    StartDateTime = DateTime.Today,
                    EndDateTime = DateTime.Today.AddDays(7)
                }
            );

            await base.SaveChangesAsync();
            return true;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Database.db");
        }
    }
}
