using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FlashTrigger : MonoBehaviour
{
    [SerializeField] GameObject flashableText;
    [SerializeField] string defaultText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (flashableText.TryGetComponent(out IFlashable flashable)){
                flashable.Flash(defaultText);
            }
        }
    }

    public void FlashText(string text)
    {
        if (flashableText.TryGetComponent(out IFlashable flashable))
        {
            flashable.Flash(text);
        }
    }
}
