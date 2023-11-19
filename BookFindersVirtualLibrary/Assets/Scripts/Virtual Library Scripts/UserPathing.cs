using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(LineRenderer))]
public class UserPathing : MonoBehaviour
{
    private LineRenderer myLineRenderer;
    private NavMeshPath myNavMeshPath;
    Vector3 lastPosition;
    Vector3 destination;

    [SerializeField] GameObject clickMarker;
    [SerializeField] Transform visualObjectsParent;

    private void Start()
    {
        lastPosition = transform.position;
        destination = Vector3.zero;

        clickMarker.SetActive(false);
        myNavMeshPath = new NavMeshPath();

        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.startWidth = 0.15f;
        myLineRenderer.endWidth = 0.15f;
        myLineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool clicked1 = Input.GetKeyDown(KeyCode.Alpha1);
        bool clicked2 = Input.GetKeyDown(KeyCode.Alpha2);
        bool clicked3 = Input.GetKeyDown(KeyCode.Alpha3);
        bool clicked4 = Input.GetKeyDown(KeyCode.Alpha4);
        bool clicked = false;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    ClickToGetLocation();
        //}

        if (clicked1)
        {
            destination = new Vector3(-22f, clickMarker.transform.position.y, 33f);
            Debug.Log("Set Navigation To Study Area 2");
        }
        else if (clicked2)
        {
            destination = new Vector3(-35f, clickMarker.transform.position.y, -35f);
            Debug.Log("Set Navigation To Material Connections");
        }
        else if (clicked3)
        {
            destination = new Vector3(30.5f, clickMarker.transform.position.y, 21.5f);
            Debug.Log("Set Navigation To a Book");
        }
        else if (clicked4)
        {
            destination = new Vector3(-3.5f, clickMarker.transform.position.y, -8.45f);
            Debug.Log("Set Navigation To Board Game Rental");
        }
        clicked = clicked1 || clicked2 || clicked3 || clicked4;

        if (destination != Vector3.zero)
        {
            if (transform.position != lastPosition || clicked)
            {
                SetDestination(destination);

                lastPosition = transform.position;
                if (myNavMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    DrawPath();
                }
                float remainingDistance = Vector3.Distance(lastPosition, destination);
                if (remainingDistance < 5)
                {
                    destination = Vector3.zero;
                }
            }

        }
        else
        {
            clickMarker.SetActive(false);
            myLineRenderer.positionCount = 0;
            destination = Vector3.zero;
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
}
