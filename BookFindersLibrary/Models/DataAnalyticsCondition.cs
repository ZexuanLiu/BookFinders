using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookFindersLibrary.Enums;

namespace BookFindersLibrary.Models
{
    public class DataAnalyticsCondition
    {
        public SheridanCampusEnum? Campus { get; set; }

        public NavigationMethodEnmu? NavigationMethod { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}