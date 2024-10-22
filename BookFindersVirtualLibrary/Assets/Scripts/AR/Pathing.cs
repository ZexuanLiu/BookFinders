using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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

    public GameObject flashingBookshelves;
    private GameObject currentFlashingBookshelf;
    private bool navigationToBookshelfSet;

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
            MeshRenderer flashingSurfaceMeshRenderer = flashingSurface.GetComponent<MeshRenderer>();

            flashingSurfaceMeshRenderer.enabled = false;
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

        InitiateBookshelfPathfindingDictionaries();

        if (BookSearchTracking.SelectedBook != null)
        {
            string bookLocationKey = $"{BookSearchTracking.SelectedBook.LocationBookShelfNum}{BookSearchTracking.SelectedBook.LocationBookShelfSide}";
            string bookName = BookSearchTracking.SelectedBook.Name;

            SetBookDestinationTo(bookLocationKey, bookName);
            navigationToBookshelfSet = true;
        }

        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        bool newPosition = lastPosition != transform.position;

        if (newPosition)
        {
            lastPosition = transform.position;
        }

        if (clickMarks[currentLocationIndex] == null && BookSearchTracking.SelectedBook == null)
        {
            myLineRenderer.positionCount = 0;
            return;
        }

        if (newPosition || lastLocationIndex != currentLocationIndex || navigationToBookshelfSet)
        {

            if (BookSearchTracking.SelectedBook == null)
            {
                UpdateHotspotDestination();
            }
            else
            {
                UpdateBookshelfDestination();
            }

            Vector3 arrowDestination = new Vector3(destination.x, arrowAboveUser.transform.position.y, destination.z);
            arrowAboveUser.transform.LookAt(arrowDestination);
            NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, myNavMeshPath);
            lastLocationIndex = currentLocationIndex;
            DrawPath();

            float remainingDistance = Vector3.Distance(lastPosition, destination);
            if (remainingDistance < 2)
            {
                FinishNavigation();
            }
        }
    }

    private void UpdateHotspotDestination()
    {
        GameObject clickMarker = clickMarks[currentLocationIndex];
        GameObject flashingSurface = flashingSurfacesPerPOI[currentLocationIndex];
        MeshRenderer flashingSurfaceMeshRenderer = flashingSurface.GetComponent<MeshRenderer>();

        if (clickMarker != null)
        {
            clickMarker.SetActive(false);
            flashingSurfaceMeshRenderer.enabled = false;
        }

        destination = clickMarker.transform.position;

        clickMarker.SetActive(true);
        clickMarker.transform.SetParent(visualObjectsParent);
        flashingSurfaceMeshRenderer.enabled = true;
    }

    private void UpdateBookshelfDestination()
    {
        arrowAboveUser.SetActive(true);
        navigationToBookshelfSet = false;
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
            MeshRenderer flashingSurfaceMeshRenderer = flashingSurface.GetComponent<MeshRenderer>();
            flashingSurfaceMeshRenderer.enabled = false;
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
                    iFlashable.Flash("Pathfinding destination set to Board Game Rental shelf");
                    arrowAboveUser.SetActive(true);
                    break;
                }
            case 2:
                {
                    iFlashable.Flash("Pathfinding destination set to door to Material Connections");
                    arrowAboveUser.SetActive(true);
                    break;
                }
        }

        if (currentFlashingBookshelf != null)
        {
            currentFlashingBookshelf.SetActive(false);
        }

    }

    public void FinishNavigation()
    {
        GameObject clickMarker = clickMarks[currentLocationIndex];
        GameObject flashingSurface = flashingSurfacesPerPOI[currentLocationIndex];

        if (clickMarker != null)
        {
            clickMarker.SetActive(false);
            MeshRenderer flashingSurfaceMeshRenderer = flashingSurface.GetComponent<MeshRenderer>();
            flashingSurfaceMeshRenderer.enabled = false;
            myLineRenderer.positionCount = 0;
        }

        switch (currentLocationIndex)
        {
            case 1:
                {
                    iFlashable.Flash("You have arrived at Board Game Rental shelf");
                    break;
                }
            case 2:
                {
                    iFlashable.Flash("You have arrived at Material Connections room");
                    break;
                }
        }

        if (currentFlashingBookshelf != null)
        {
            currentFlashingBookshelf.SetActive(false);

            float remainingDistance = Vector3.Distance(transform.position, new Vector3(destination.x, transform.position.y, destination.z));
            if (remainingDistance < 2)
            {
                iFlashable.Flash($"You have arrived at the book '{BookSearchTracking.SelectedBook.Name}'");
            }
            else
            {
                iFlashable.Flash($"You have cancelled navigation to {BookSearchTracking.SelectedBook.Name}");
            }
        }
        destination = Vector3.zero;

        BookSearchTracking.SelectedBook = null;

        currentLocationIndex = 0;
        arrowAboveUser.SetActive(false);
    }

    public void InitiateBookshelfPathfindingDictionaries()
    {
        BookSearchTracking.BookPathfindingSurfaces.Clear();
        BookSearchTracking.BookPathfindLocations.Clear();

        float widthOfShelves = 0.653f;
        float distanceBetweenShelves = 0.835f;
        float distanceToNewShelf = widthOfShelves + distanceBetweenShelves;

        Vector3 startingShelvesCoord1 = new Vector3(-3.46f - (distanceToNewShelf / 2), 1.076f, 3.06f);
        Vector3 startingShelvesCoord2 = new Vector3(8.424f + (distanceToNewShelf / 2), 1.076f, 11.26f);

        foreach (Transform bookshelf in flashingBookshelves.transform)
        {
            GameObject bookshelfObject = bookshelf.gameObject;
            string bookshelfNum = bookshelf.name;

            int bookshelfNumAsInt = 0;
            if (!int.TryParse(bookshelfNum, out bookshelfNumAsInt))
            {
                Debug.Log($"Could not parse bookshelfNum '{bookshelfNum}' to int");
            }

            if (bookshelfNumAsInt > 20)
            {
                Debug.Log($"bookshelfNum '{bookshelfNum}' was too large and was not counted in pathfinding");
            }

            foreach (Transform bookshelfSide in bookshelf)
            {
                GameObject bookshelfSideObject = bookshelfSide.gameObject;
                string bookshelfSideAlpha = bookshelfSideObject.name;

                string key = $"{bookshelfNum}{bookshelfSideAlpha}";
                BookSearchTracking.BookPathfindingSurfaces.Add(key, bookshelfSideObject);
                //Debug.Log($"Surface = {key} - {bookshelfSideObject}");

                // Setting up search locations for the surface of the bookshelf and the waypoint to it.
                // Note that these values have been measured previously
                // Because of the way our bookshelf numbers are mapped, there has to be separate logic for each range
                float newX = float.MinValue;
                Vector3 locationPoint = Vector3.zero;
                if (bookshelfNumAsInt >= 1 && bookshelfNumAsInt <= 9)
                {
                    newX = startingShelvesCoord1.x;
                    newX += (bookshelfNumAsInt - 1) * distanceToNewShelf;
                    if (bookshelfSideAlpha.Equals("B"))
                    {
                        newX += distanceToNewShelf;
                    }
                    locationPoint = new Vector3(newX, startingShelvesCoord1.y, startingShelvesCoord1.z);
                }
                else if (bookshelfNumAsInt >= 10 && bookshelfNumAsInt <= 18)
                {
                    newX = startingShelvesCoord2.x;
                    newX -= (bookshelfNumAsInt - 10) * distanceToNewShelf;
                    if (bookshelfSideAlpha.Equals("A"))
                    {
                        newX -= distanceToNewShelf;
                    }
                    locationPoint = new Vector3(newX, startingShelvesCoord2.y, startingShelvesCoord2.z);
                }

                BookSearchTracking.BookPathfindLocations.Add(key, locationPoint);
                //Debug.Log($"Location = {key} - {locationPoint}");

                bookshelfSideObject.SetActive(false);
            }
        }
    }

    public void SetBookDestinationTo(string bookshelfLocationKey, string bookName)
    {
        if (!BookSearchTracking.BookPathfindingSurfaces.ContainsKey(bookshelfLocationKey))
        {
            Debug.Log($"No Key To Navigate To '{bookshelfLocationKey}'");
            iFlashable.Flash($"Navigation to {Environment.NewLine}'{bookName}'{Environment.NewLine} is currently not supported");
            return;
        }

        string message = $"Set Navigation To: {Environment.NewLine}\"{bookName}\"";

        GameObject bookshelfSurface = BookSearchTracking.BookPathfindingSurfaces[bookshelfLocationKey];
        Vector3 bookshelfDestination = BookSearchTracking.BookPathfindLocations[bookshelfLocationKey];

        Debug.Log($"Going To {bookshelfLocationKey} at {bookshelfDestination.ToString()}");

        destination = bookshelfDestination;
        iFlashable.Flash(message);

        currentFlashingBookshelf = bookshelfSurface;
        bookshelfSurface.SetActive(true);
    }
}
