using System;
using System.Collections.Generic;
using System.Text;

namespace BookFinders.Model
{
     public class book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
    }
}
