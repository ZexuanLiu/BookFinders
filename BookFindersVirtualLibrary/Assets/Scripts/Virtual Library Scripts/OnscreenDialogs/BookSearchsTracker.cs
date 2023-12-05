using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookFindersLibrary.Models;
using UnityEngine;

namespace Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs
{
    public static class BookSearchsTracker
    {
        public static int Id { get; set; }
        public static string BookName { get; set; }
        public static string BookAuthor { get; set; }

        public static string BookLocationCode { get; set; }

        public static Dictionary<string, Vector3> BookPathfindLocations { get; set; }
        public static Dictionary<string, GameObject> BookPathfindingSurfaces { get; set; }

        static BookSearchsTracker()
        {
            BookPathfindLocations = new Dictionary<string, Vector3>();
            BookPathfindingSurfaces = new Dictionary<string, GameObject>();
        }

        public static void SetClickedBook(int id, string bookName, string bookAuthor, string bookLocationCode)
        {
            Id = id;
            BookName = bookName;
            BookAuthor = bookAuthor;
            BookLocationCode = bookLocationCode;
            BookSelectedUpdated();
        }

        public static event Action onBookSelectedUpdated;
        public static void BookSelectedUpdated()
        {
            if (onBookSelectedUpdated != null)
            {
                onBookSelectedUpdated();
            }
        }

        


    }
}
