using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashingSurface : MonoBehaviour
{
    [SerializeField] MeshRenderer flashObjectMesh;

    private float flashFadeIn = 1f;
    private float flashStay = 0.1f;
    private float flashFadeOut = 1f;

    private float timePassed;
    private float totalTextTime;

    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        totalTextTime = flashFadeIn + flashStay + flashFadeOut;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color currentColor = flashObjectMesh.material.color;

        timePassed += Time.deltaTime;


        if (timePassed < flashFadeIn)
        {
            float newOpacity = (timePassed / flashFadeIn);
            flashObjectMesh.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, newOpacity);
        }
        else if (timePassed < (flashFadeIn + flashStay))
        {
            float newOpacity = 1;
            flashObjectMesh.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, newOpacity);
        }
        else if (timePassed < totalTextTime)
        {
            float newOpacity = 1 - ((timePassed - (flashFadeIn + flashStay)) / flashFadeOut);
            flashObjectMesh.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, newOpacity);
        }
        else
        {
            timePassed = 0;
        }
    }
}
