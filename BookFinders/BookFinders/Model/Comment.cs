using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFindersLibrary.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        public int ThumbsUp {get; set;}
        public string UserName { get; set; }
        public string Description { get; set; }
        public DateTime PostDateTime { get; set; }
    }
}