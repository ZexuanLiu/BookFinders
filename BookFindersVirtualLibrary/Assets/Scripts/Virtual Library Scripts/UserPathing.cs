using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

interface IFindingPathTo
{
    public Vector3 ArrowDestination();

    public void CycleTargets();
}

[RequireComponent(typeof(FlashTrigger))]
[RequireComponent(typeof(LineRenderer))]
public class UserPathing : MonoBehaviour, IFindingPathTo
{
    private LineRenderer myLineRenderer;
    private NavMeshPath myNavMeshPath;
    Vector3 lastPosition;
    Vector3 destination;
    Vector3 arrowDestination;

    [SerializeField] GameObject clickMarker;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform visualObjectsParent;

    [SerializeField] bool useControllerCycle;

    private bool navigationStarted;
    private FlashTrigger trigger;
    private string currentDestinationText;

    private List<Vector3> locations;
    private int currentLocationIndex;
    private int currentIndexSwitchedTo;

    private void Start()
    {
        lastPosition = transform.position;
        destination = Vector3.zero;

        clickMarker.SetActive(false);
        arrow.SetActive(false);
        myNavMeshPath = new NavMeshPath();

        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.startWidth = 0.15f;
        myLineRenderer.endWidth = 0.15f;
        myLineRenderer.positionCount = 0;

        navigationStarted = false;
        trigger = GetComponent<FlashTrigger>();
        currentDestinationText = string.Empty;

        locations = new List<Vector3>()
        {
            Vector3.zero,
            new Vector3(-22f, clickMarker.transform.position.y, 33f),
            new Vector3(-35f, clickMarker.transform.position.y, -35f),
            new Vector3(30.5f, clickMarker.transform.position.y, 21.5f),
            new Vector3(-3.5f, clickMarker.transform.position.y, -8.45f)
        };
        currentLocationIndex = -1;
        currentIndexSwitchedTo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool clicked0 = false;
        bool clicked1 = false;
        bool clicked2 = false;
        bool clicked3 = false;
        bool clicked4 = false;
        bool clicked = false;

        if (useControllerCycle)
        {
            if (currentIndexSwitchedTo != currentLocationIndex)
            {
                currentLocationIndex = currentIndexSwitchedTo;

                clicked0 = currentLocationIndex == 0;
                clicked1 = currentLocationIndex == 1;
                clicked2 = currentLocationIndex == 2;
                clicked3 = currentLocationIndex == 3;
                clicked4 = currentLocationIndex == 4;

                //Debug.Log(currentLocationIndex);
            }
        }
        else
        {
            clicked1 = Input.GetKeyDown(KeyCode.Alpha1);
            clicked2 = Input.GetKeyDown(KeyCode.Alpha2);
            clicked3 = Input.GetKeyDown(KeyCode.Alpha3);
            clicked4 = Input.GetKeyDown(KeyCode.Alpha4);
        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //    ClickToGetLocation();
        //}

        string message = string.Empty;
        if (clicked0)
        {
            navigationStarted = false;
            currentLocationIndex = 0;
            destination = locations[currentLocationIndex];
        }
        if (clicked1)
        {
            currentDestinationText = "Study Area 2";
            message = "Set Navigation To " + currentDestinationText;
            currentLocationIndex = 1;
            destination = locations[currentLocationIndex];
            trigger.FlashText(message);
        }
        else if (clicked2)
        {
            currentDestinationText = "Material Connections";
            message = "Set Navigation To " + currentDestinationText;
            currentLocationIndex = 2;
            destination = locations[currentLocationIndex];
            trigger.FlashText(message);
        }
        else if (clicked3)
        {
            currentDestinationText = "a Book";
            message = "Set Navigation To " + currentDestinationText;
            currentLocationIndex = 3;
            destination = locations[currentLocationIndex];
            trigger.FlashText(message);
        }
        else if (clicked4)
        {
            currentDestinationText = "Board Game Rental";
            message = "Set Navigation To " + currentDestinationText;
            currentLocationIndex = 4;
            destination = locations[currentLocationIndex];
            trigger.FlashText(message);
        }
        clicked = clicked0 || clicked1 || clicked2 || clicked3 || clicked4;

        if (destination != Vector3.zero)
        {
            navigationStarted = true;

            if (transform.position != lastPosition || clicked)
            {
                SetDestination(destination);
                arrowDestination = new Vector3(destination.x, arrow.transform.position.y, destination.z);
                arrow.transform.LookAt(arrowDestination);

                lastPosition = transform.position;
                if (myNavMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    DrawPath();
                }
                float remainingDistance = Vector3.Distance(lastPosition, destination);
                if (remainingDistance < 7.5)
                {
                    destination = Vector3.zero;
                }
            }

        }
        else
        {
            if (navigationStarted)
            {
                message = "You have arrived at " + currentDestinationText;
                trigger.FlashText(message);
            }
            clickMarker.SetActive(false);
            myLineRenderer.positionCount = 0;
            destination = Vector3.zero;
            arrow.SetActive(false);
            currentDestinationText = string.Empty;
            navigationStarted = false;
        }

    }

    private void ClickToGetLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            Debug.Log(hit.point);
        }
    }

    private void SetDestination(Vector3 target)
    {
        clickMarker.SetActive(true);
        clickMarker.transform.SetParent(visualObjectsParent);
        clickMarker.transform.position = new Vector3 (target.x, clickMarker.transform.position.y, target.z);
        NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, myNavMeshPath);
    }

    private void DrawPath()
    {
        myLineRenderer.positionCount = myNavMeshPath.corners.Length;
        myLineRenderer.SetPosition(0, transform.position);

        if (myNavMeshPath.corners.Length < 2)
        {
            return;
        }
       
        for (int i = 1; i < myNavMeshPath.corners.Length; i++)
        {
            Vector3 corner = myNavMeshPath.corners[i];
            myLineRenderer.SetPosition(i, corner);
        }

    }

    public Vector3 ArrowDestination()
    {
        return arrowDestination;
    }

    public void CycleTargets()
    {
        currentIndexSwitchedTo = (currentIndexSwitchedTo + 1) % locations.Count;
    }
}
