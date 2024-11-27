using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookFindersVirtualLibrary.Models
{
    public class UserTrackingSession
    {
        public int Id { get; set; }
        public string Campus { get; set; }
        public List<UserTrackingInstance> Locations { get; set; }
        public DateTime TimeStarted { get; set; }
        public DateTime TimeEnded { get; set; }
    }
}
