using System;
using System.Collections.Generic;
using System.Text;

namespace BookFindersLibrary.Models
{
    public class BookResponse
    {
        public List<Doc> docs { get; set; }
    }
    public class Doc
    {
        public Pnx pnx { get; set; }
        public Delivery delivery { get; set; }
    }
    public class Delivery
    {
        public BestLocation? bestlocation{ get; set; }
        public string? almaOpenurl {get; set;}
    }
    public class BestLocation
    {
        public string? libraryCode;
        public string? callNumber; 
    }
    public class Pnx
    {
        public PnxSort sort { get; set; }
        public PnxSearch search { get; set; }
        public PnxAdData addata { get; set; }
        public PnxDisplay display { get; set; }
    }
    public class PnxSort
    {
        public List<string> creationdate { get; set; }
        public List<string> author { get; set; }
        public List<string> title { get; set; }
        public PnxSort()
        {
            // Initialize lists to avoid null reference exceptions
            creationdate = new List<string>();
            author = new List<string>();
            title = new List<string>();
        }
    }
    public class PnxSearch
    {
        public List<string> description { get; set; }
        public List<string> creationdate { get; set; }
    }
    public class PnxDisplay
    {
        public List<string> publisher {get; set;}
    }
    public class PnxAdData
    {
        public List<string> isbn {get; set;}
        public PnxAdData()
        {
            isbn = new List<string>();
        }
    }
    
}