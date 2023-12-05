using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BookDetailsBack : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject bookSearchView;
    [SerializeField] GameObject bookDetailsView;

    public void OnPointerClick(PointerEventData eventData)
    {
        bookSearchView.SetActive(true);
        bookDetailsView.SetActive(false);
    }

}
