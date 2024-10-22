using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

interface IFlashableAR
{
    public void Flash(string text);
}

public class FlashingText : MonoBehaviour, IFlashableAR
{
    [SerializeField] GameObject textObject;
    [SerializeField] Image backgroundOfTextObject;
    [SerializeField] float flashFadeIn = 1f;
    [SerializeField] float flashStay = 1f;
    [SerializeField] float flastFadeOut = 1f;

    private static volatile Queue<string> flashingTextStack = new Queue<string>();
    private static readonly string defaultText = "Default Text";

    private TextMeshProUGUI textMesh;
    float originalBackgroundOpacity;

    private float timePassed;
    private float totalTextTime;
    private bool timeToRun;

    public void Flash(string text)
    {
        flashingTextStack.Enqueue(text);
        textMesh.text = text;
        new WaitUntil(() => object.ReferenceEquals(flashingTextStack.TryPeek(out string nextFlashText), text));
        timeToRun = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        textMesh = textObject.GetComponent<TextMeshProUGUI>();
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);

        originalBackgroundOpacity = backgroundOfTextObject.color.a;
        backgroundOfTextObject.color = new Color(backgroundOfTextObject.color.r, backgroundOfTextObject.color.g, backgroundOfTextObject.color.b, 0);

        totalTextTime = flashFadeIn + flashStay + flastFadeOut;
        textMesh.text = defaultText;
        timePassed = 0;
        timeToRun = false;
    }

    void Update()
    {
        if (!timeToRun)
        {
            return;
        }

        timePassed += Time.deltaTime;
        if (timePassed < flashFadeIn)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, timePassed / flashFadeIn);
            backgroundOfTextObject.color = new Color(backgroundOfTextObject.color.r, backgroundOfTextObject.color.g, backgroundOfTextObject.color.b, (timePassed / flashFadeIn) * originalBackgroundOpacity);
        }
        else if (timePassed < (flashFadeIn + flashStay))
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);
            backgroundOfTextObject.color = new Color(backgroundOfTextObject.color.r, backgroundOfTextObject.color.g, backgroundOfTextObject.color.b, 1 * originalBackgroundOpacity);
        }
        else if (timePassed < totalTextTime)
        {
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1 - (timePassed - (flashFadeIn + flashStay) / flastFadeOut));
            backgroundOfTextObject.color = new Color(backgroundOfTextObject.color.r, backgroundOfTextObject.color.g, backgroundOfTextObject.color.b, (1 - (timePassed - (flashFadeIn + flashStay) / flastFadeOut)) * originalBackgroundOpacity);
        }
        else
        {
            timePassed = 0;
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0);
            backgroundOfTextObject.color = new Color(backgroundOfTextObject.color.r, backgroundOfTextObject.color.g, backgroundOfTextObject.color.b, 0);
            flashingTextStack.Dequeue();
            timeToRun = false;
        }
    }
}
