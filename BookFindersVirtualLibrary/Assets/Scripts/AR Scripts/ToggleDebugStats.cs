using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleDebugStats : MonoBehaviour, IPointerClickHandler
{

    public List<GameObject> debugComponents;

    void Start()
    {
        foreach (var component in debugComponents)
        {
            if (component != null)
            {
                component.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var component in debugComponents)
        {
            if (component != null)
            {
                component.SetActive(!component.activeSelf);
            }
        }
    }
}
