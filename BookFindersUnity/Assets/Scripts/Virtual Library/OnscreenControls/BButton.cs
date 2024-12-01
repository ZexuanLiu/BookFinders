using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject userPathingObject;
    [SerializeField] GameObject flashableText;

    private IFindingPathTo findingPath;
    private IFlashable iFlashable;

    private float timePassed;
    private const float CONFIRMATION_TIME = 5;

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

        if (flashableText.TryGetComponent(out IFlashable flashable))
        {
            iFlashable = flashable;
        }
        else
        {
            throw new Exception("FlashableText has no IFlashable");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timePassed > 0)
        {
            timePassed += Time.deltaTime;

            if (timePassed > CONFIRMATION_TIME)
            {
                timePassed = 0;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!findingPath.IsNavigatingToLocation())
        {
            iFlashable.Flash("You are currently not navigating to a location, nothing to cancel...");
            return;
        }

        if (timePassed == 0)
        {
            iFlashable.Flash("Are you sure you wish to cancel? Press 'B' again to confirm.");
            timePassed += Time.deltaTime;
            return;
        }

        if (timePassed <= CONFIRMATION_TIME)
        {
            findingPath.FinishNavigation();
            timePassed = 0;
        }
    }
}
