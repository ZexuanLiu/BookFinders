using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFindersLibrary.Models
{
    public class UserLocations
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DestinationId {get;set;}
        public double XCoordinate {get;set;}
        public double YCoordinate {get;set;}
        
    }
}