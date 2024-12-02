using System;
using BookFindersLibrary.Models;

namespace BookFindersWebApp.Models
{
	public class DataAnalyticsModel
	{
        public DataAnalyticsCondition Condition { get; set; } = new DataAnalyticsCondition();
        public DataAnalyticsRepository DataModel { get; set; } = new DataAnalyticsRepository();
    }
}

