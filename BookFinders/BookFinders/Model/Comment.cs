using System;
using System.Collections.Generic;
using System.Text;

namespace BookFinders.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string BookId { get; set; }
        public int ThumbsUp { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public DateTime postDateTime { get; set; }
    }
}
