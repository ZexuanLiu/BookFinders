using System;
using System.Collections.Generic;
using System.Text;

namespace BookFinders.Model
{
    public enum Role
    {
        Student,
        Librarian,
        Faculty
       
    }
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Role Authorization { get; set; }
        public string Password { get; set; }

    }
}
