using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class PositionReset : MonoBehaviour, IPointerClickHandler
{
    public GameObject arSessionObject;
    public GameObject xrOrigin;

    public Camera mainCamera;
    public GameObject cameraOffset;

    public GameObject xrOriginParent;

    public ARSession arSession;

    private Vector3 xrOriginPosition;
    private Quaternion xrOriginRotation;

    // Start is called before the first frame update
    void Start()
    {
        xrOriginPosition = xrOrigin.transform.position;
        xrOriginRotation = xrOrigin.transform.rotation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        xrOriginParent.transform.position = xrOriginPosition;

        arSessionObject.transform.position = xrOriginPosition;

        mainCamera.transform.position = xrOriginPosition;
        cameraOffset.transform.position = xrOriginPosition;

        xrOrigin.transform.position = xrOriginPosition;
        xrOrigin.transform.rotation = xrOriginRotation;

        arSession.Reset();
    }
}
