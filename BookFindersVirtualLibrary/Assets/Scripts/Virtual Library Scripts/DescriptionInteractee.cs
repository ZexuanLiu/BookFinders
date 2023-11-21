using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionInteractee : MonoBehaviour, IInteractable
{

    [SerializeField] string hotspotName;
    [SerializeField] string hotspotDescription;

    public string GetTitle()
    {
        return hotspotName;
    }


    public string GetDescription()
    {
        return hotspotDescription;
    }

    public void Interact()
    {
        Debug.Log($"You are looking at {hotspotName}{Environment.NewLine}{hotspotDescription}");
    }
}
