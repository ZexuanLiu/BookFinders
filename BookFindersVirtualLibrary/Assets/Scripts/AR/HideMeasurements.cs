using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMeasurements : MonoBehaviour
{
    public List<GameObject> measurementObjects;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var measurement in measurementObjects)
        {
            measurement.gameObject.SetActive(false);
        }
    }

}
