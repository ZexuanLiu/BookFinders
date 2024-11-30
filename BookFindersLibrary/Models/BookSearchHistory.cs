using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookFindersLibrary.Enums;

namespace BookFindersLibrary.Models
{
    public class BookSearchHistory
    {
        public int Id { get; set; }
        public SheridanCampusEnum Campus { get; set; }
        public NavigationMethodEnmu NavigationMethod {get; set;}
        public string Subject { get; set; }
        public DateTime SearchDate {  get; set; }
    }
}