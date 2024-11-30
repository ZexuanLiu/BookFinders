using System.Collections;
using System.Collections.Generic;
using BookFindersVirtualLibrary.Models;
using UnityEngine;

public static class BookManager
{
    public static string rawSearchTerm { get; set; }
    public static Book currentBook { get; set; }
    public static int searchDropdownValue { get; set; }

    public static List<Book> SearchResultBooks { get; set; }
}
