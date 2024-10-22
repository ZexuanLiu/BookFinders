using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CyclePaths : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject userPathingObject;

    private IFindingPathToAR findingPath;

    // Start is called before the first frame update
    void Start()
    {
        if (userPathingObject.TryGetComponent(out IFindingPathToAR findingPathInterface))
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
        findingPath.CycleLocations();
    }
}
