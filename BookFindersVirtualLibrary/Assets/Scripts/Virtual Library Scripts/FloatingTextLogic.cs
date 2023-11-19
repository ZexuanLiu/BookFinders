using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextLogic : MonoBehaviour
{
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject interactText;
    [SerializeField] float showRange = 25f;

    private TextMeshPro titleTextMesh;
    private TextMeshPro interactTextMesh;

    // Start is called before the first frame update
    void Start()
    {
        titleTextMesh = titleText.GetComponent<TextMeshPro>();
        interactTextMesh = interactText.GetComponent<TextMeshPro>();

        titleTextMesh.gameObject.SetActive(false);
        interactTextMesh.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = new Vector3(Camera.main.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);

        float distance = Vector3.Distance(transform.position, playerPosition);

        if (distance < showRange)
        {
            titleTextMesh.gameObject.SetActive(true);
            interactTextMesh.gameObject.SetActive(true);
            this.transform.LookAt(2 * transform.position - new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y-4, Camera.main.transform.position.z));
        }
        else
        {
            titleTextMesh.gameObject.SetActive(false);
            interactTextMesh.gameObject.SetActive(false);
        }
    }
}
