using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GPSLocation : MonoBehaviour
{

    public TextMeshProUGUI GPSStatus;
    public TextMeshProUGUI latitudeValue;
    public TextMeshProUGUI longitudeValue;
    public TextMeshProUGUI altitudeValue;
    public TextMeshProUGUI horizontalAccuracy;
    public TextMeshProUGUI timeStampValue;
    public TextMeshProUGUI position;

    public Transform cameraPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc()
    {
        
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        // location available by user
        if (!Input.location.isEnabledByUser)
        {
            GPSStatus.text = $"Location service is not enabled";
            yield break;
        }
        Input.location.Start(1f,1f);

        // wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // service didn't init in 20 secds
        if (maxWait < 1)
        {
            GPSStatus.text = $"Could not init after {maxWait} secodns";
            yield break;
        }

        // Connection Failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = $"Connection Failed, unable to determine location";
            yield break;
        }
        else
        {
            GPSStatus.text = $"Started";
            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
        }
    }

    private void UpdateGPSData()
    {

        

        if (Input.location.status == LocationServiceStatus.Running)
        {
            GPSStatus.text = $"Running ({DateTime.Now})";
            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracy.text = Input.location.lastData.horizontalAccuracy.ToString();
            timeStampValue.text = Input.location.lastData.timestamp.ToString(); 
            position.text = cameraPos.transform.position.ToString(); 
        }
        else
        {
            GPSStatus.text = $"Stopped";
        }
    }

}
