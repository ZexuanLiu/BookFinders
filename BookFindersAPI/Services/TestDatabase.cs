using BookFindersAPI.Interfaces;
using BookFindersLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BookFindersAPI.Services
{
    public class TestDatabase : DbContext, IDatabase
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
