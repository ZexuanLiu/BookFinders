using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SearchClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_InputField thisInput;

    [SerializeField] GameObject bookSearchView;
    [SerializeField] GameObject bookDetailsView;

    public void OnPointerClick(PointerEventData eventData)
    {
        bookSearchView.SetActive(true);
        bookDetailsView.SetActive(false);

        string textInput = thisInput.text;
        Debug.Log($"{textInput}");
    }
}
