using System;
using System.Collections.Generic;
using System.Text;

namespace BookFinders.Model
{
    class BookResponse
    {
        public List<Doc> docs { get; set; }
    }
    class Doc
    {
        public Pnx pnx { get; set; }
    }
    class Pnx
    {
        public PnxSort sort { get; set; }
        public PnxSearch search { get; set; }
        public PnxLinks links { get; set; }
    }
    class PnxSort
    {
        public List<string> creationdate { get; set; }
        public List<string> author { get; set; }
        public List<string> title { get; set; }
    }
    class PnxSearch
    {
        public List<string> description { get; set; }
    }
    class PnxLinks 
    {
        public List<string> thumbnail { get; set; }
    }

}
