using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractableAR
{
    public void Interact();

    public string GetName();

    public string GetShownText();
}

public class InteractableObject : MonoBehaviour, IInteractableAR
{
    [SerializeField] string objName;
    [SerializeField] string objShownText;

    public void Interact()
    {
        Debug.Log($"You are looking at {objName}{Environment.NewLine}{objShownText}");
    }

    public string GetName()
    {
        return objName;
    }

    public string GetShownText()
    {
        return objShownText;
    }
}
