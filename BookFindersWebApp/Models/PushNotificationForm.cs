using System.ComponentModel.DataAnnotations;

namespace BookFindersWebApp.Models
{
    public class PushNotificationForm
    {

        [Required(ErrorMessage = "Title for Push Notification Is Required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description for Push Notification Is Required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Starting Date/Time for Push Notification Is Required")]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "Ending Date/Time for Push Notification Is Required")]
        public DateTime EndDateTime { get; set; }

    }
}
