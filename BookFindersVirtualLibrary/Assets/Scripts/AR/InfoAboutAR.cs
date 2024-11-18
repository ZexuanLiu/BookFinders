using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoAboutAR : MonoBehaviour, IPointerClickHandler
{
    public GameObject infoPopup;

    public void OnPointerClick(PointerEventData eventData)
    {
        infoPopup.SetActive(true);
    }
}
