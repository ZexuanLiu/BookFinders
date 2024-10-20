using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;

public class PositionReset : MonoBehaviour, IPointerClickHandler
{
    public GameObject arSession;

    public Camera mainCamera;

    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        initialCameraPosition = mainCamera.transform.position;
        initialCameraRotation = mainCamera.transform.rotation;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        arSession.transform.position = initialCameraPosition;

        mainCamera.transform.position = initialCameraPosition;
        mainCamera.transform.rotation = initialCameraRotation;
    }
}
