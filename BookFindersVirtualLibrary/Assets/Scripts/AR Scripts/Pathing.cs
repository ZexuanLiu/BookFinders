using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

interface IFindingPathToAR
{ 
    public void CycleLocations();

    public void FinishNavigation();

}

[RequireComponent(typeof(LineRenderer))]
public class Pathing : MonoBehaviour, IFindingPathToAR
{

    private LineRenderer myLineRenderer;
    private NavMeshPath myNavMeshPath;

    private Vector3 destination;
    private Vector3 lastPosition;

    public Transform visualObjectsParent;
    public List<GameObject> clickMarks;
    public List<GameObject> flashingSurfacesPerPOI;

    public GameObject flashingText;
    private IFlashableAR iFlashable;

    private int currentLocationIndex = 0;
    private int lastLocationIndex = 0;

    public GameObject arrowAboveUser;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        foreach (var clickMarker in clickMarks)
        {
            clickMarker.SetActive(false);
        }
        foreach (var flashingSurface in flashingSurfacesPerPOI)
        {
            flashingSurface.SetActive(false);
        }

        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.startWidth = 0.15f;
        myLineRenderer.endWidth = 0.15f;
        myLineRenderer.positionCount = 0;
        myNavMeshPath = new NavMeshPath();

        clickMarks.Reverse();
        clickMarks.Add(null);
        clickMarks.Reverse();

        flashingSurfacesPerPOI.Reverse();
        flashingSurfacesPerPOI.Add(null);
        flashingSurfacesPerPOI.Reverse();

        if (flashingText.TryGetComponent(out IFlashableAR flashable))
        {
            iFlashable = flashable;
        }
        else
        {
            throw new Exception("User has no IFlashable");
        }

        arrowAboveUser.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool newPosition = lastPosition != transform.position;

        if (newPosition)
        {
            lastPosition = transform.position;
        }

        if (clickMarks[currentLocationIndex] == null)
        {
            myLineRenderer.positionCount = 0;
            return;
        }

        if (newPosition || lastLocationIndex != currentLocationIndex)
        {
            UpdateDestination();

            lastLocationIndex = currentLocationIndex;
            DrawPath();

            float remainingDistance = Vector3.Distance(lastPosition, destination);
            if (remainingDistance < 1)
            {
                destination = Vector3.zero;
                FinishNavigation();
            }
        }
    }

    private void UpdateDestination()
    {
        GameObject clickMarker = clickMarks[currentLocationIndex];
        GameObject flashingSurface = flashingSurfacesPerPOI[currentLocationIndex];
        if (clickMarker != null)
        {
            clickMarker.SetActive(false);
            flashingSurface.SetActive(false);
        }

        destination = clickMarker.transform.position;

        Vector3 arrowDestination = new Vector3(destination.x, arrowAboveUser.transform.position.y, destination.z);
        arrowAboveUser.transform.LookAt(arrowDestination);

        clickMarker.SetActive(true);
        clickMarker.transform.SetParent(visualObjectsParent);
        flashingSurface.SetActive(true);
        NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, myNavMeshPath);
    }

    private void DrawPath()
    {
        Vector3 linePosition = new Vector3(transform.position.x, destination.y, transform.position.z);

        myLineRenderer.positionCount = myNavMeshPath.corners.Length;
        myLineRenderer.SetPosition(0, linePosition);

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

    public void CycleLocations()
    {
        GameObject clickMarker = clickMarks[currentLocationIndex];
        GameObject flashingSurface = flashingSurfacesPerPOI[currentLocationIndex];
        if (clickMarker != null)
        {
            clickMarker.SetActive(false);
            flashingSurface.SetActive(false);
        }

        currentLocationIndex = (currentLocationIndex + 1) % clickMarks.Count;

        switch (currentLocationIndex)
        {
            case 0:
                {
                    iFlashable.Flash("No destination for pathfinding selected");
                    arrowAboveUser.SetActive(false);
                    break;
                }
            case 1:
                {
                    iFlashable.Flash("Pathfinding destination set to Board Game Rental");
                    arrowAboveUser.SetActive(true);
                    break;
                }
            case 2:
                {
                    iFlashable.Flash("Pathfinding destination set to door to Material Connections");
                    arrowAboveUser.SetActive(true);
                    break;
                }
            case 3:
                {
                    iFlashable.Flash("Pathfinding destination set to Art Hallway Back Door");
                    arrowAboveUser.SetActive(true);
                    break;
                }
        }
        
    }

    public void FinishNavigation()
    {
        GameObject clickMarker = clickMarks[currentLocationIndex];
        GameObject flashingSurface = flashingSurfacesPerPOI[currentLocationIndex];
        if (clickMarker != null) {
            clickMarker.SetActive(false);
            flashingSurface.SetActive(false);
            myLineRenderer.positionCount = 0;
        }

        currentLocationIndex = 0;
        arrowAboveUser.SetActive(false);
    }
}
