using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoCyclePics : MonoBehaviour, IPointerClickHandler
{

    public List<GameObject> examplePics;

    public bool isRightArrow;

    public void OnPointerClick(PointerEventData eventData)
    {

        int examplePicsCount = examplePics.Count;
        int nextActiveIndex = -1;

        for (int i = 0; i < examplePicsCount; i++)
        {
            GameObject pic = examplePics[i];
            if (pic.activeSelf)
            {
                nextActiveIndex = i - 1;
                if (isRightArrow)
                {
                    nextActiveIndex = i + 1;
                }

                if (nextActiveIndex >= examplePicsCount) {
                    nextActiveIndex = 0;
                }
                if (nextActiveIndex < 0)
                {
                    nextActiveIndex = examplePicsCount - 1;
                }
            }
        }

        if (nextActiveIndex > -1)
        {
            for (int i = 0; i < examplePicsCount; i++)
            {
                GameObject pic = examplePics[i];
                pic.SetActive(i == nextActiveIndex);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < examplePics.Count; i++)
        {
            GameObject pic = examplePics[i];
            pic.SetActive(i == 0);
        }
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
