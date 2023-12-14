using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookDetailsControl : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshName;
    [SerializeField] TextMeshProUGUI textMeshAuthor;
    [SerializeField] TextMeshProUGUI textMeshLocationCode;
    [SerializeField] TextMeshProUGUI textMeshDescription;

    // Start is called before the first frame update
    void Start()
    {
        textMeshName.text = BookSearchsTracker.SelectedBook.Name;
        textMeshAuthor.text = BookSearchsTracker.SelectedBook.Author;
        textMeshLocationCode.text = BookSearchsTracker.SelectedBook.LocationCode;
        textMeshDescription.text = BookSearchsTracker.SelectedBook.Description;
    }

    void OnEnable()
    {
        textMeshName.text = BookSearchsTracker.SelectedBook.Name;
        textMeshAuthor.text = BookSearchsTracker.SelectedBook.Author;
        textMeshLocationCode.text = BookSearchsTracker.SelectedBook.LocationCode;
        textMeshDescription.text = BookSearchsTracker.SelectedBook.Description;
    }
}
