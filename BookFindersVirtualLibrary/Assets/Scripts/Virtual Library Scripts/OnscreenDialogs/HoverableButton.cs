using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] Image buttonImage;
    [SerializeField] Sprite buttonImageInvert;
    [SerializeField] TextMeshProUGUI buttonText = null;
    [SerializeField] bool isButtonDisabled;

    private Color initialTextColor;
    private Color initialButtonColor;
    private Sprite buttonImageOriginal;

    private Color activeButtonColor;
    private bool hasStartScriptRun = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isButtonDisabled)
        {
            return;
        }
        if (buttonText != null)
        {
            buttonText.color = initialButtonColor;
        }
        
        if (buttonImageInvert != null)
        {
            buttonImage.sprite = buttonImageInvert;
        }
        else
        {
            buttonImage.color = activeButtonColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isButtonDisabled)
        {
            return;
        }
        if (buttonText != null)
        {
            buttonText.color = initialTextColor;
        }

        if (buttonImageInvert != null)
        {
            buttonImage.sprite = buttonImageOriginal;
        }
        else
        {
            buttonImage.color = initialButtonColor;
        }
    }

    void OnDisable()
    {
        if (hasStartScriptRun)
        {
            OnPointerExit(null);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isButtonDisabled)
        {
            buttonImage.color = Color.gray;
        }
        initialButtonColor = buttonImage.color;
        if (buttonText != null)
        {
            initialTextColor = buttonText.color;
            activeButtonColor = initialTextColor;
        }
        else
        {
            activeButtonColor = Color.white;
        }
        buttonImageOriginal = buttonImage.sprite;
        hasStartScriptRun = true;

    }

}
