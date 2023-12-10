using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

interface IHoverableButton
{
    public void SetActive();

    public void SetInactive();
}

public class HoverableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IHoverableButton
{

    [SerializeField] Image buttonImage;
    [SerializeField] TextMeshProUGUI buttonText = null;
    [SerializeField] bool isButtonDisabled;

    private Color initialTextColor;
    private Color initialButtonColor;

    private Color activeButtonColor;
    private bool isButtonActive;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isButtonDisabled || isButtonActive)
        {
            return;
        }
        if (buttonText != null)
        {
            buttonText.color = initialButtonColor;
        }
        buttonImage.color = activeButtonColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isButtonDisabled || isButtonActive)
        {
            return;
        }
        if (buttonText != null)
        {
            buttonText.color = initialTextColor;
        }
        buttonImage.color = initialButtonColor;
    }

    //private void OnDisable()
    //{
    //    if (isButtonDisabled || isButtonActive)
    //    {
    //        return;
    //    }
    //    if (buttonText != null)
    //    {
    //        buttonText.color = initialTextColor;
    //    }
    //    buttonImage.color = initialButtonColor;
    //}

    // Start is called before the first frame update
    void Start()
    {
        if (isButtonDisabled)
        {
            buttonImage.color = Color.gray;
        }
        if (buttonText != null)
        {
            initialTextColor = buttonText.color;
        }
        initialButtonColor = buttonImage.color;
        activeButtonColor = Color.white;
        isButtonActive = false;
    }

    public void SetActive()
    {
        OnPointerEnter(null);
        isButtonActive = true;
    }

    public void SetInactive()
    {
        isButtonActive = false;
        OnPointerExit(null);
    }

}
