using Assets.Scripts.Virtual_Library_Scripts.OnscreenDialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

interface IFindingPathTo
{
    public Vector3 ArrowDestination();

    public void CycleLocations();

    public void SetBookDestinationTo(string bookshelfLocationKey, string bookName);

    public void FinishNavigation();

}

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

    [SerializeField] GameObject bookshelves;
    private GameObject currentFlashingBookshelf;

    private bool navigationStarted;
    private string currentDestinationText;

    private List<Vector3> locations;
    private List<string> locationLabels;
    private int currentLocationIndex;
    private int currentIndexSwitchedTo;

    private bool destinationUpdated;

    [SerializeField] GameObject flashableText;
    private IFlashable iFlashable;

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
        currentDestinationText = string.Empty;

        if (flashableText.TryGetComponent(out IFlashable flashable))
        {
            iFlashable = flashable;
        }
        else
        {
            throw new Exception("User has no IFlashable");
        }

        locationLabels = new List<string>()
        {
            string.Empty,
            "Study Area 1",
            "Study Area 2",
            "Study Area 3",
            "Material ConneXion",
            "Board Game Rental",
            "Printer"
        };
        locations = new List<Vector3>()
        {
            Vector3.zero, // Default no search location
            new Vector3(-4.5f, clickMarker.transform.position.y, -33.5f), // Study Area 1
            new Vector3(-22f, clickMarker.transform.position.y, 4.25f), // Study Area 2
            new Vector3(-22f, clickMarker.transform.position.y, 33f), // Study Area 3
            new Vector3(-35f, clickMarker.transform.position.y, 12.1f), // Material ConneXion
            new Vector3(-3.5f, clickMarker.transform.position.y, -8.45f),  // Board Game Rental
            new Vector3(42.5f, clickMarker.transform.position.y, -0.5f) // Printer
        };
        currentLocationIndex = -1;
        currentIndexSwitchedTo = 0;

        InitiateBookshelfPathfindingDictionaries();

        if (locationLabels.Count != locations.Count)
        {
            throw new Exception("Locations and LocationLabels lists not the same length!");
        }

        if (BookSearchsTracker.SelectedBook != null)
        {
            string localLocationCode = BookSearchsTracker.SelectedBook.LocationBookShelfNum + BookSearchsTracker.SelectedBook.LocationBookShelfSide;

            SetBookDestinationTo(localLocationCode, BookSearchsTracker.SelectedBook.Name);

            BookSearchsTracker.BookSearchInProgress = true;
        }

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        bool wasCycleSwitched = currentIndexSwitchedTo != currentLocationIndex;

        if (wasCycleSwitched)
        {
            currentLocationIndex = currentIndexSwitchedTo;

            if (currentLocationIndex != 0)
            {
                currentDestinationText = locationLabels[currentLocationIndex];
                destination = locations[currentLocationIndex];
                string message = "Set Navigation To " + currentDestinationText;
                FlashText(message);
            }
        }

        if (destination != Vector3.zero)
        {
            navigationStarted = true;

            if (transform.position != lastPosition || wasCycleSwitched || destinationUpdated)
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
                string message = "You have arrived at " + currentDestinationText;
                FlashText(message);
            }
            clickMarker.SetActive(false);
            myLineRenderer.positionCount = 0;
            destination = Vector3.zero;
            arrow.SetActive(false);
            currentDestinationText = string.Empty;
            navigationStarted = false;
        }
        destinationUpdated = false;

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

    public void FlashText(string text)
    {
        iFlashable.Flash(text);
    }

    public Vector3 ArrowDestination()
    {
        return arrowDestination;
    }

    public void CycleLocations()
    {
        currentIndexSwitchedTo = (currentIndexSwitchedTo + 1) % locations.Count;
        destinationUpdated = true;

        BookSearchsTracker.BookSearchInProgress = false;
        if (currentFlashingBookshelf != null)
        {
            currentFlashingBookshelf.SetActive(false);
        }
    }

    public void SetBookDestinationTo(string bookshelfLocationKey, string bookName)
    {
        if (!BookSearchsTracker.BookPathfindingSurfaces.ContainsKey(bookshelfLocationKey))
        {
            Debug.Log($"No Key To Navigate To '{bookshelfLocationKey}'");
            FlashText($"Navigation to {Environment.NewLine}'{bookName}'{Environment.NewLine} is currently not supported");
            return;
        }

        destinationUpdated = true;
        navigationStarted = true;
        currentDestinationText = bookName;
        string message = $"Set Navigation To: {Environment.NewLine}\"{currentDestinationText}\"";
        Debug.Log($"Going To {bookshelfLocationKey}");

        GameObject bookshelfSurface = BookSearchsTracker.BookPathfindingSurfaces[bookshelfLocationKey];
        Vector3 bookshelfDestination = BookSearchsTracker.BookPathfindLocations[bookshelfLocationKey];

        destination = bookshelfDestination;
        FlashText(message);

        currentFlashingBookshelf = bookshelfSurface;
        bookshelfSurface.SetActive(true);
    }

    public void FinishNavigation()
    {
        if (navigationStarted)
        {
            destination = Vector3.zero;
            string message = $"You have cancelled navigation to: {Environment.NewLine}\"{currentDestinationText}\"";
            FlashText(message);
            clickMarker.SetActive(false);
            myLineRenderer.positionCount = 0;
            arrow.SetActive(false);
            currentDestinationText = string.Empty;
            navigationStarted = false;
        }
        if (currentFlashingBookshelf != null)
        {
            currentFlashingBookshelf.SetActive(false);
        }
    }

    public void InitiateBookshelfPathfindingDictionaries()
    {
        BookSearchsTracker.BookPathfindingSurfaces.Clear();
        BookSearchsTracker.BookPathfindLocations.Clear();


        Vector3 startingShelvesCoord1 = new Vector3(-8f, 1.25f, 13f);
        Vector3 startingShelvesCoord2 = new Vector3(40.25f, 1.25f, 39.25f);

        float widthOfShelves = 4.25f;
        float distanceBetweenShelves = 1.25f;
        float distanceToNewShelf = widthOfShelves + distanceBetweenShelves;

        foreach (Transform bookshelf in bookshelves.transform)
        {
            GameObject bookshelfObject = bookshelf.gameObject;
            string bookshelfNum = bookshelf.name;

            int bookshelfNumAsInt = 0;
            if (!int.TryParse(bookshelfNum, out bookshelfNumAsInt)){
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
                BookSearchsTracker.BookPathfindingSurfaces.Add(key, bookshelfSideObject);
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
                        newX += widthOfShelves;
                    }
                    locationPoint = new Vector3(newX, startingShelvesCoord1.y, startingShelvesCoord1.z);
                }
                else if (bookshelfNumAsInt >= 10 && bookshelfNumAsInt <= 18)
                {
                    newX = startingShelvesCoord2.x;
                    newX -= (bookshelfNumAsInt - 10) * distanceToNewShelf;
                    if (bookshelfSideAlpha.Equals("A"))
                    {
                        newX -= widthOfShelves;
                    }
                    locationPoint = new Vector3(newX, startingShelvesCoord2.y, startingShelvesCoord2.z);
                }

                BookSearchsTracker.BookPathfindLocations.Add(key, locationPoint);
                //Debug.Log($"Location = {key} - {locationPoint}");

                bookshelfSideObject.SetActive(false);
            }
        }
    }
}
