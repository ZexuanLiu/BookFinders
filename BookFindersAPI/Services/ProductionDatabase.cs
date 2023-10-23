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
        private DbSet<UserLocations> _locations {get; set;}
        public async Task<PushNotification> AddPushNotification(PushNotification pushNotification)
        {
            _pushNotifications.Add(pushNotification);
            await base.SaveChangesAsync();
            return pushNotification;
        }
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? host = Environment.GetEnvironmentVariable("bookfindersDBHost");
            string? user = Environment.GetEnvironmentVariable("bookfindersDBUser");
            string? pass = Environment.GetEnvironmentVariable("bookfindersDBPassword");

            optionsBuilder.UseNpgsql($"Host={host};Username={user};Password={pass}");
        }
    }
}
