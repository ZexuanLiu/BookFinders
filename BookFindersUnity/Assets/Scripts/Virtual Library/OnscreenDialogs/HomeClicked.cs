using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HomeClicked : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        BookSearchsTracker.SelectedBook = null;
        BookSearchsTracker.SearchResultBooks = null;

        SceneManager.LoadScene("Home");
    }
}
