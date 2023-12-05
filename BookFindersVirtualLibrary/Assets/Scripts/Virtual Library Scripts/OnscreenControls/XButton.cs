using Assets.Scripts.Virtual_Library_Scripts.OnscreenControls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class XButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject userPathingObject;

    private IFindingPathTo findingPath;

    // Start is called before the first frame update
    void Start()
    {
        if (userPathingObject.TryGetComponent(out IFindingPathTo findingPathInterface))
        {
            findingPath = findingPathInterface;
        }
        else
        {
            throw new Exception("UserPathing has no IFindingPathTo");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        findingPath.CycleTargets();
    }
}
