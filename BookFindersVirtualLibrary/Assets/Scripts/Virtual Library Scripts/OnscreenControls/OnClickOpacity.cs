using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OnClickOpacity : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private Image thisImage;
    private int clickOpacity = 1;
    private Color clickedColor;
    private Color initialColor;

    public void OnPointerDown(PointerEventData eventData)
    {
        thisImage.color = clickedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        thisImage.color = initialColor;
    }

    // Start is called before the first frame update
    void Start()
    {
        thisImage = GetComponent<Image>();
        initialColor = thisImage.color;
        clickedColor = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, clickOpacity);
    }

}
