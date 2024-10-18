using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ExitBookNavigation : MonoBehaviour, IPointerClickHandler
{

    public TextMeshProUGUI selfButtonText;

    public GameObject btnLibraryHotspots;

    public GameObject pathingSource;
    private IFindingPathToAR iFindingPathToAR;

    // Start is called before the first frame update
    void Start()
    {
        if (BookSearchTracking.SelectedBook != null)
        {
            btnLibraryHotspots.SetActive(false);
            selfButtonText.text = "End Book Navigation";
        }
        else
        {
            btnLibraryHotspots.SetActive(true);
            selfButtonText.text = "Exit Augmented Reality";
        }

        if (pathingSource.TryGetComponent(out IFindingPathToAR findingPathtoAR))
        {
            iFindingPathToAR = findingPathtoAR;
        }
        else
        {
            throw new Exception("User has no IFindingPathToAR");
        }

    }

    void Update()
    {
        if (BookSearchTracking.SelectedBook != null)
        {
            btnLibraryHotspots.SetActive(false);
            selfButtonText.text = "End Book Navigation";
        }
        else
        {
            btnLibraryHotspots.SetActive(true);
            selfButtonText.text = "Exit Augmented Reality";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (BookSearchTracking.SelectedBook != null)
        {
            iFindingPathToAR.FinishNavigation();
        }
        else
        {
            SceneManager.LoadScene("BrowseBooks");
        }
    }
}
