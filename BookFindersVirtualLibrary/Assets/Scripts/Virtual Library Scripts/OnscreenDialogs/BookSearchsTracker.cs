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
        public static book SelectedBook { get; private set; }

        public static List<book> SearchResultBooks { get; set; }

        public static Dictionary<string, Vector3> BookPathfindLocations { get; set; }
        public static Dictionary<string, GameObject> BookPathfindingSurfaces { get; set; }

        public static bool BookSearchInProgress { get; set; }

        static BookSearchsTracker()
        {
            BookPathfindLocations = new Dictionary<string, Vector3>();
            BookPathfindingSurfaces = new Dictionary<string, GameObject>();

            SelectedBook = new book();
        }

        public static void SetClickedBook(int id)
        {
            if (id >= SearchResultBooks.Count)
            {
                Debug.Log($"{id} is larger than search results");
                return;
            }

            SelectedBook = SearchResultBooks[id];
            Id = id;
        }

    }
}
