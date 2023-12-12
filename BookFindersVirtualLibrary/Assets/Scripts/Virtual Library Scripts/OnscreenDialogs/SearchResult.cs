using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

interface ISearchResult
{
    public void SetName(string name);
    public void SetAuthor(string author);
    public void SetId(int id);

    public void OnStart();
}

public class SearchResult : MonoBehaviour, IPointerClickHandler, ISearchResult
{
    [SerializeField] GameObject bookSearchView;
    [SerializeField] GameObject bookDetailsView;

    private TextMeshProUGUI textMeshName;
    private TextMeshProUGUI textMeshAuthor;

    private int id;
    private string bookName;
    private string bookAuthor;

    public void OnPointerClick(PointerEventData eventData)
    {
        BookSearchsTracker.SetClickedBook(id);

        bookSearchView.SetActive(false);
        bookDetailsView.SetActive(true);
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetName(string name)
    {
        this.bookName = name;
        textMeshName.text = name;
    }


    public void SetAuthor(string author)
    {
        this.bookAuthor = author;
        textMeshAuthor.text = author;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStart()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Name"))
            {
                textMeshName = child.gameObject.GetComponent<TextMeshProUGUI>();
            }
            else if (child.name.Equals("Author"))
            {
                textMeshAuthor = child.gameObject.GetComponent<TextMeshProUGUI>();
            }
        }
    }
}
