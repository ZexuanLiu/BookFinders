using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookDetailsControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshName;
    [SerializeField] TextMeshProUGUI textMeshAuthor;

    // Start is called before the first frame update
    void Start()
    {
        BookSearchsTracker.onBookSelectedUpdated += OnBookSelectedUpdated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBookSelectedUpdated()
    {
        textMeshName.text = BookSearchsTracker.BookName;
        textMeshAuthor.text = BookSearchsTracker.BookAuthor;
    }
}
