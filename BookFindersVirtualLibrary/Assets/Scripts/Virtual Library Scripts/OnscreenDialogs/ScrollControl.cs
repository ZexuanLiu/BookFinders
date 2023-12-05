using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IScrollBoxControl
{
    public void AddNewSearchResult(int id, string name, string author);
}

public class ScrollControl : MonoBehaviour, IScrollBoxControl
{
    [SerializeField] GameObject templateSearchResult;
    [SerializeField] GameObject messageNoResultsFound;
    [SerializeField] GameObject messageNoSearchesYet;

    private List<GameObject> searchResultChildren = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        templateSearchResult.SetActive(false);
        messageNoResultsFound.SetActive(false);

        AddNewSearchResult(1, "Hello", "World");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewSearchResult(int id, string name, string author)
    {
        GameObject duplicate = Instantiate(templateSearchResult);
        duplicate.transform.SetParent(transform, false);
        duplicate.SetActive(true);
        searchResultChildren.Add(duplicate);

        if (duplicate.TryGetComponent(out ISearchResult iSearchResult))
        {
            iSearchResult.OnStart();
            iSearchResult.SetId(id);
            iSearchResult.SetName(name);
            iSearchResult.SetAuthor(author);
        }
        else
        {
            throw new Exception("UI Element has no ISearchResult");
        }


    }
}
