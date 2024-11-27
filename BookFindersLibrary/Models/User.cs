using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookFindersLibrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
        public UserLogin UserLogin{  get; set; }
    }
}
