using System;
using System.Collections.Generic;
using System.Text;

namespace BookFindersLibrary.Models
{
    //public enum BookShelfSide
    //{
    //    A,
    //    B,
    //    NotAvailable
    //}
     public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<string> Isbns { get; set; }
        public string Subject {get; set;}
        public string Description { get; set; }
        public string Publisher {get; set;}
        public string PublishYear {get; set;}
        public string? OnlineResourceURL {get; set;}
        public string? ImageLink { get; set; }
        public string? LocationCode {get; set; }
        public string? LibraryCode {get; set; }
        public string? LocationBookShelfNum {get; set;}
        public string? LocationBookShelfSide {get; set;}
        // public int? LocationBookShelfRow {get; set;}
        // public int? LocationBookShelfColumn {get; set;}
    }
}
