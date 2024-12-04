using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFindersLibrary.Models
{
    public class  BookSearchHistoryResponse
    {
        public List<string> TopSubjects { get; set; }
        public List<int> SubjectCounts { get; set; }
    }
}