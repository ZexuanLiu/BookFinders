using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartMenuHamburger : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] RectTransform sideMenuRectTransformation;

    private float width;
    private float startPositionX;
    private float startingAnchoredPositionX;

    // Start is called before the first frame update
    void Start()
    {
        width = Screen.width;
    }

    public void OnDrag(PointerEventData eventData)
    {
        sideMenuRectTransformation.anchoredPosition = new Vector2(Mathf.Clamp(startingAnchoredPositionX - (startPositionX - eventData.position.x), GetMinPosition(), GetMaxPosition()), 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        startPositionX = eventData.position.x;
        startingAnchoredPositionX = sideMenuRectTransformation.anchoredPosition.x;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(HandleMenuSlide(.25f, sideMenuRectTransformation.anchoredPosition.x, isAfterHalfPoint() ? GetMinPosition() : GetMaxPosition()));
    }

    private float GetMinPosition()
    {
        return width / 2;
    }

    private float GetMaxPosition()
    {
        return width * 1.5f;
    }

    private bool isAfterHalfPoint()
    {
        return sideMenuRectTransformation.anchoredPosition.x < width;
    }

    private IEnumerator HandleMenuSlide(float slideTime, float startingX, float targetX)
    {
        for (float i = 0; i < slideTime; i+= .025f)
        {
            sideMenuRectTransformation.anchoredPosition = new Vector2(Mathf.Lerp(startingX, targetX, i / slideTime), 0);
            yield return new WaitForSecondsRealtime(.025f);
        }
    }

}
