using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NavigationMenu : MonoBehaviour
{
    public GameObject panel;
    void Start()
    {
        panel.SetActive(false);
    }
    public void OnNavigationButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void TogglePanelVisibility()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
