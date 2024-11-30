using System;
using System.Collections.Generic;
using System.Text;

namespace BookFindersVirtualLibrary.Models
{
    public class BookSearchHistory
    {
        public int Id { get; set; }
        public SheridanCampusEnum Campus { get; set; }
        public NavigationMethodEnmu NavigationMethod { get; set; }
        public string Subject { get; set; }
        public DateTime SearchDate { get; set; }
    }

    public enum SheridanCampusEnum
    {
        Trafalgar,
        Davis,
        HMC,
        Unknown
    }
    public enum NavigationMethodEnmu
    {
        VirtualLibrary,
        AugmentedReality,
        Unknown
    }
}

