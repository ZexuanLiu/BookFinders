using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BookDetailsOk : MonoBehaviour, IPointerClickHandler
{
    public GameObject bookDetailsPopup;

    public void OnPointerClick(PointerEventData eventData)
    {
        bookDetailsPopup.SetActive(false);
    }
}
