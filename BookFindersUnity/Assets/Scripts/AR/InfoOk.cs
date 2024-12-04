using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoOk : MonoBehaviour, IPointerClickHandler
{
    public GameObject infoPopup;

    public void OnPointerClick(PointerEventData eventData)
    {
        infoPopup.SetActive(false);
    }
}
