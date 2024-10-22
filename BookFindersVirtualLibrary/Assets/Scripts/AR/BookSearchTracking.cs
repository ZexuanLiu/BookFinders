using BookFindersVirtualLibrary.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BookSearchTracking
{
    public static Book SelectedBook { get; set; }

    public static Dictionary<string, Vector3> BookPathfindLocations { get; set; }
    public static Dictionary<string, GameObject> BookPathfindingSurfaces { get; set; }

    static BookSearchTracking()
    {
        BookPathfindLocations = new Dictionary<string, Vector3>();
        BookPathfindingSurfaces = new Dictionary<string, GameObject>();
    }
}
