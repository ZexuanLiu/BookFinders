using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookFindersVirtualLibrary.Models
{
    public class UserTrackingInstance
    {
        public int Id { get; set; }
        public Coordinate Coordinate { get; set; }
        public DateTime PostDateTime { get; set; }
    }
}
