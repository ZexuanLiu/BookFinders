using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleARComponents : MonoBehaviour, IPointerClickHandler
{

    public List<GameObject> ARComponents;

    void Start()
    {
        foreach (var component in ARComponents)
        {
            RecurrsivelyHideGameObjects(component);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var component in ARComponents)
        {
            RecurrsivelyHideGameObjects(component);
        }
    }

    private void RecurrsivelyHideGameObjects(GameObject gameObject)
    {
        if (gameObject != null)
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = !meshRenderer.enabled;
            }
            
            Transform parentTransform = gameObject.transform;

            foreach (Transform child in parentTransform)
            {
                RecurrsivelyHideGameObjects(child.gameObject);
            }

        }
    }
}
