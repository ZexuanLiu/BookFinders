using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SyncSearch : MonoBehaviour
{
    [SerializeField] TMP_InputField[] searchBars;

    private void Start()
    {
        foreach (TMP_InputField searchBar in searchBars)
        {
            searchBar.onValueChanged.AddListener(SearchTextUpdated);
        }
    }

    public void SearchTextUpdated(string searchText)
    {
        foreach (TMP_InputField searchBar in searchBars)
        {
            searchBar.text = searchText;
        }
    }

}
