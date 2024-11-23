using System;

namespace BookFindersVirtualLibrary.Models
{
    public class PushNotification
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; } 
    }
}
