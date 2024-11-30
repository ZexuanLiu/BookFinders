using BookFindersVirtualLibrary.Models;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToBookSearch : MonoBehaviour
{
    public TMP_InputField bookSearchTextArea;
    public TMP_Dropdown searchOptionDropdown;
    public Image searchIcon;

    // Start is called before the first frame update
    void Start()
    {
        Button searchButton = searchIcon.GetComponent<Button>();
        if (searchButton != null)
        {
            searchButton.onClick.AddListener(OnSearchIconClicked);
        }
        bookSearchTextArea.text = BookManager.rawSearchTerm;
        searchOptionDropdown.value = BookManager.searchDropdownValue;
    } 

    void OnSearchIconClicked()
    {
        BookManager.searchDropdownValue = searchOptionDropdown.value;
        BookManager.rawSearchTerm = bookSearchTextArea.text;

        SceneManager.LoadScene("BrowseBooks");
    }
}
