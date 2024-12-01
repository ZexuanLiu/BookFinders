using System;
using BookFindersLibrary.Models;

namespace BookFindersWebApp.Models
{
	public class DataAnalystViewModel
	{
        public DataAnalystCondition Condition { get; set; } = new DataAnalystCondition();
        public DataAnalystModel DataModel { get; set; } = new DataAnalystModel();
    }
}

