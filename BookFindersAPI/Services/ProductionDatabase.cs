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
        public async Task<PushNotification> AddPushNotification(PushNotification pushNotification)
        {
            _pushNotifications.Add(pushNotification);
            await base.SaveChangesAsync();
            return pushNotification;
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? host = Environment.GetEnvironmentVariable("bookfindersDBHost");
            string? user = Environment.GetEnvironmentVariable("bookfindersDBUser");
            string? pass = Environment.GetEnvironmentVariable("bookfindersDBPassword");

            optionsBuilder.UseNpgsql($"Host={host};Username={user};Password={pass}");
        }
    }
}
