using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingLabel : MonoBehaviour
{
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject interactText;
    [SerializeField] GameObject backgroundPlane;

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(2 * transform.position - new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
    }
}
