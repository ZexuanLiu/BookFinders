using BookFindersAPI.Interfaces;
using BookFindersLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BookFindersAPI.Services
{
    public class ProductionDatabase : DbContext, IDatabase
    {
        private DbSet<PushNotification> _pushNotifications { get; set; }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? host = Environment.GetEnvironmentVariable("bookfindersDBHost");
            string? user = Environment.GetEnvironmentVariable("bookfindersDBUser");
            string? pass = Environment.GetEnvironmentVariable("bookfindersDBPassword");

            optionsBuilder.UseNpgsql($"Host={host};Username={user};Password={pass}");
        }
    }
}
