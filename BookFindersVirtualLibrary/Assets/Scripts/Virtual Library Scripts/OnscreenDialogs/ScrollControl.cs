using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public interface IScrollBoxControl
{
    public void AddNewSearchResult(int id, string name, string author);

    public void ClearSearchResults();

    public void SetNoResultsFound();

    public void SetSearchingMessage();

    public void ClearSearchingMessage();
}

public class ScrollControl : MonoBehaviour, IScrollBoxControl
{
    [SerializeField] GameObject templateSearchResult;
    [SerializeField] GameObject messageNoResultsFound;
    [SerializeField] GameObject messageNoSearchesYet;
    [SerializeField] GameObject messageSearching;
    [SerializeField] GameObject templates;

    private List<GameObject> searchResultChildren = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        templateSearchResult.SetActive(false);
        messageNoResultsFound.SetActive(false);
        messageSearching.SetActive(false);
        templateSearchResult.transform.SetParent(templates.transform);
        messageNoResultsFound.transform.SetParent(templates.transform);
        messageSearching.transform.SetParent(templates.transform);
        messageNoSearchesYet.transform.SetParent(templates.transform);

        messageNoSearchesYet.transform.SetParent(transform);
        messageNoSearchesYet.SetActive(true);

        templates.SetActive(false);
        //AddNewSearchResult(1, "Hello", "World");
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

    public void ClearSearchResults()
    {
        messageNoResultsFound.SetActive(false);
        messageSearching.SetActive(false);
        messageNoSearchesYet.SetActive(false);
        messageNoResultsFound.transform.SetParent(templates.transform);
        messageSearching.transform.SetParent(templates.transform);
        messageNoSearchesYet.transform.SetParent(templates.transform);

        searchResultChildren.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetNoResultsFound()
    {
        ClearSearchResults();
        messageNoResultsFound.transform.SetParent(transform);
        messageNoResultsFound.SetActive(true);
    }

    public void SetSearchingMessage()
    {
        ClearSearchResults();
        messageSearching.SetActive(true);
        messageSearching.transform.SetParent(gameObject.transform);
    }

    public void ClearSearchingMessage()
    {
        messageSearching.SetActive(false);
        messageSearching.transform.SetParent(templates.transform);
    }
}
