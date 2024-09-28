using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartSideButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image thisButton;
    [SerializeField] TextMeshProUGUI thisText;
    [SerializeField] ButtonType buttonType;

    private static GameObject currentActiveCenter;

    private static Dictionary<ButtonType, GameObject> centerTypes = new Dictionary<ButtonType, GameObject>();

    public void Start()
    {
        ButtonDeselect.AddToButtons(thisButton);
        ButtonDeselect.AddToButtonTexts(thisText);

        if (currentActiveCenter == null)
        {
            currentActiveCenter = GameObject.Find("Center/Home");
            centerTypes.Add(ButtonType.Home, currentActiveCenter);
            centerTypes.Add(ButtonType.Controls, GameObject.Find("Center/Controls"));
            centerTypes.Add(ButtonType.Help, GameObject.Find("Center/Help"));
            centerTypes.Add(ButtonType.Settings, GameObject.Find("Center/Settings"));

            foreach (GameObject centers in centerTypes.Values)
            {
                centers.SetActive(false);
            }
            
            currentActiveCenter.SetActive(true);
        }

        if (buttonType == ButtonType.Home)
        {
            ButtonDeselect.ResetButtons();
            thisButton.color = Color.white;
            thisText.color = new Color32(1, 55, 103, 255);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ButtonDeselect.ResetButtons();
        thisButton.color = Color.white;
        thisText.color = new Color32(1, 55, 103, 255);

        if (buttonType == ButtonType.Quit)
        {
            Application.Quit();
            return;
        }

        GameObject newCenter = centerTypes[buttonType];
        if (newCenter != null && newCenter != currentActiveCenter)
        {
            currentActiveCenter.SetActive(false);
            newCenter.SetActive(true);
            currentActiveCenter = newCenter;
        }
    }
}



public enum ButtonType
{
    Home, Controls, Help, Settings, Quit
}

static class ButtonDeselect
{
    private static List<Image> buttons = new List<Image>();
    private static List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>();

    public static void ResetButtons()
    {
        foreach (Image button in buttons)
        {
            button.color = new Color32(1, 55, 103, 255);
        }
        foreach (TextMeshProUGUI buttonText in buttonTexts)
        {
            buttonText.color = Color.white;
        }
    }

    public static void AddToButtons(Image newButton)
    {
        buttons.Add(newButton);
    }

    public static void AddToButtonTexts(TextMeshProUGUI newButtonText)
    {
        buttonTexts.Add(newButtonText);
    }
}