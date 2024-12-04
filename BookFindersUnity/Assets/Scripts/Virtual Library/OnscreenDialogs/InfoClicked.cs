using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoClicked : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject infoPopup;

    public void OnPointerClick(PointerEventData eventData)
    {
        infoPopup.SetActive(!infoPopup.activeSelf);
    }

    // Start is called before the first frame update
    void Start()
    {
        infoPopup.SetActive(false);
    }

}
