
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BookFinders.Model
{
    public class CommentResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public ObservableCollection<Comment> data { get; set; }
        public object errors { get; set; }
    }
}
