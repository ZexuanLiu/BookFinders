using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BookDetailsAR : MonoBehaviour, IPointerClickHandler
{
    public GameObject bookDetails;

    [SerializeField] TextMeshProUGUI textMeshName;
    [SerializeField] TextMeshProUGUI textMeshAuthor;
    [SerializeField] TextMeshProUGUI textMeshLocationCode;
    [SerializeField] TextMeshProUGUI textMeshDescription;

    // Start is called before the first frame update
    void Start()
    {
        bookDetails.SetActive(false);

        if (BookSearchTracking.SelectedBook == null)
        {
            return;
        }
        textMeshName.text = BookSearchTracking.SelectedBook.Name;
        textMeshAuthor.text = BookSearchTracking.SelectedBook.Author;
        textMeshLocationCode.text = BookSearchTracking.SelectedBook.LocationCode;
        textMeshDescription.text = BookSearchTracking.SelectedBook.Description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bookDetails.SetActive(true);
    }
}
